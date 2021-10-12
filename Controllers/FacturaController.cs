using ApiNetCore.Dtos;
using ApiNetCore.Repositories;
using ApiNetCore.Helpers;
using ApiNetCore.Entities;
using ApiNetCore.Middlewares;
using ApiNetCore.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;

namespace ApiNetCore.Controllers
{

    [ApiController]
    [Route("api/factura")]
    [Authorize]
    public class FacturaController : ControllerBase
    {
        private readonly IEmpresasRepository empresasRepository;

        private readonly IUsuariosRepository usuariosRepository;

        private readonly IClientesRepository clientesRepository;

        private readonly IFacturaEmitidaRepository facturaEmitidaRepository;

        private readonly ITiposIdentificacionRepository tiposIdentificacionRepository;

        private readonly IProductosRepository productosRepository;

        private readonly ITarifasIvaRepository tarifasIvaRepository;

        private readonly IBlobService blobService;

        private readonly IConverter converter;

        public FacturaController(
                IEmpresasRepository empresasRepository, 
                IUsuariosRepository usuariosRepository, 
                IClientesRepository clientesRepository,
                IFacturaEmitidaRepository facturaEmitidaRepository,
                ITiposIdentificacionRepository tiposIdentificacionRepository,
                IProductosRepository productosRepository,
                ITarifasIvaRepository tarifasIvaRepository,
                IBlobService blobService,
                IConverter converter
            )
        {
            this.empresasRepository = empresasRepository;
            this.usuariosRepository = usuariosRepository;
            this.clientesRepository = clientesRepository;
            this.facturaEmitidaRepository = facturaEmitidaRepository;
            this.tiposIdentificacionRepository = tiposIdentificacionRepository;
            this.productosRepository = productosRepository;
            this.tarifasIvaRepository = tarifasIvaRepository;
            this.blobService = blobService;
            this.converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult<FacturaEmitidaInsertadaDto>> PostAsync(GuardarFacturaDto facturaDto)
        {
            var trabajoXml = new TrabajoXml(blobService);
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var usuario = await usuariosRepository.ObtenerPorIdAsync(Guid.Parse(uid));
            var empresa = await empresasRepository.GetAsync(Guid.Parse(usuario.IdEmpresa));
            var fecha = facturaDto.FechaEmision.Split('-');
            var fechaAuxiliar = $"{fecha[2]}/{fecha[1]}/{fecha[0]}";
            var cliente = await clientesRepository.GetAsync(Guid.Parse(facturaDto.IdCliente));
            var tipoIdentificacion = await tiposIdentificacionRepository.GetAsync(Guid.Parse(cliente.TipoIdentificacion));
            var comunes = new Comunes();
            var claveAcceso = comunes.generarClaveAcceso(fecha[2] + fecha[1] + fecha[0], "01", empresa);
            var facturaEmitida = new FacturaEmitida
            {
                ContribuyenteEspecial = empresa.ContribuyenteEspecial,
                RegimenMicroempresas = empresa.RegimenMicroempresas,
                Ambiente = empresa.Ambiente,
                TipoEmision = empresa.TipoEmision,
                RazonSocial = empresa.RazonSocial,
                NombreComercial = empresa.NombreComercial,
                Ruc = empresa.Ruc,
                ClaveAcceso = claveAcceso,
                CodDoc = "01",
                Estab = empresa.Establecimiento,
                PtoEmi = empresa.PuntoEmision,
                Secuencial = empresa.SecuencialFactura.PadLeft(9,'0'),
                DirMatriz = empresa.DireccionMatriz,
                FechaEmision = fechaAuxiliar,
                DirEstablecimiento = empresa.DireccionEstablecimiento,
                ObligadoContabilidad = empresa.ObligadoContabilidad,
                TipoIdentificacionComprador = tipoIdentificacion.Codigo,
                RazonSocialComprador = cliente.RazonSocial,
                IdentificacionComprador = cliente.NumeroIdentificacion,
                TotalSinImpuestos = facturaDto.DetalleValores.TotalSinImpuestos,
                TotalDescuento = facturaDto.DetalleValores.TotalDescuento,
                Propina = facturaDto.DetalleValores.Propina,
                TotalIva = facturaDto.DetalleValores.TotalIva,
                ImporteTotal = facturaDto.DetalleValores.ImporteTotal,
                Moneda = "DOLAR",
                EstadoComprobante = "Creada",
                FechaRegistro = DateTimeOffset.Now,
                IdUsuario = uid,
                IdCliente = cliente.Id.ToString(),
                FormaPago = ObtenerFormaPago(facturaDto.FormaPago),
                ImpuestosComprobante = MapearImpuestos(facturaDto.ImpuestosDetalle),
                DatosAdicionales = MapearDatosAdicionales(facturaDto.DatosAdicionales),
                Detalles = await MapearDetalles(facturaDto.Detalles)
            };
            await facturaEmitidaRepository.CreateAsync(facturaEmitida);
            var comprobante = trabajoXml.GenerarXmlComprobante(facturaEmitida, empresa.PathCertificado, empresa.ClaveFirma);
            ServiciosSri serviciosSri = new();
            string urlEnvio = $"https://{(facturaEmitida.Ambiente == 2 ? "cel":"celcer")}.sri.gob.ec/comprobantes-electronicos-ws/RecepcionComprobantesOffline?wsdl";
            var respuesta = serviciosSri.EnviarComprobante(comprobante, urlEnvio);
            respuesta.EstadoComprobante = serviciosSri.RecuperarEstadoComprobante(respuesta.Estado);
            return Ok(new FacturaEmitidaInsertadaDto(
                Ok: true,
                Msg: "",
                FacturaEmitida: facturaEmitida
            ));
        }

        // [HttpGet]
        // [Route("hello")]
        // [AllowAnonymous]
        // public async Task<IActionResult> Hello()
        // {
        //     TrabajoPdf objPdf = new(blobService);
        //     IDocument document = await objPdf.CreateDocument("Hello world", "<h1>Hola Mundo</h1>");
        //     byte[] content = converter.Convert(document);
        //     return File(content, "application/pdf", "hello2.pdf");
        // }

        private FormaPago ObtenerFormaPago(FormasPagoDto formasPagoDto)
        {
            FormaPago formaPago = new();
            if (formasPagoDto.TipoFormaPago == "")
            {
                formaPago.Codigo = "20";
                formaPago.Plazo = Convert.ToInt32(formasPagoDto.Plazo);
            } else {
                var codigoSplit = formasPagoDto.TipoFormaPago.Split('-');
                formaPago.Codigo = codigoSplit[0];
                formaPago.Plazo = 0;
            }
            formaPago.UnidadTiempo = formasPagoDto.TipoPlazo;
            formaPago.Valor = formasPagoDto.ValorPago;
            return formaPago;
        }

        private async Task<List<Detalle>> MapearDetalles(List<DetallesDto> detallesDto)
        {
            List<Detalle> detalles = new();

            foreach (var detalleDto in detallesDto)
            {
                var producto = await productosRepository.GetProductoAsync(Guid.Parse(detalleDto.Id));
                var tarifaIva = await tarifasIvaRepository.GetTarifaIvaAsync(Guid.Parse(detalleDto.TarifaIva));
                var totalIva = (tarifaIva.ValorPorcentaje * detalleDto.Subtotal) / 100;
                detalles.Add(new Detalle{
                    CodigoPrincipal = producto.CodigoPrincipal,
                    Descripcion = producto.Descripcion,
                    Cantidad = detalleDto.Cantidad,
                    PrecioUnitario = detalleDto.ValorUnitario,
                    Descuento = 0m,
                    TotalSinImpuesto = detalleDto.Subtotal,
                    ValorImpuesto = Convert.ToDecimal(string.Format("{0:.00}", totalIva)),
                    ImpuestoDetalle = new ImpuestoComprobante
                    {
                        CodigoImpuesto = "2",
                        CodigoPorcentaje = tarifaIva.Codigo,
                        BaseImponible = detalleDto.Subtotal,
                        Valor = Convert.ToDecimal(string.Format("{0:.00}", totalIva)),
                        Tarifa = tarifaIva.ValorPorcentaje
                    }
                });
            };
            return detalles;
        }

        private List<ImpuestoComprobante> MapearImpuestos(List<ImpuestosDetalleDto> impuestosDetalleDtos)
        {
            List<ImpuestoComprobante> impuestoComprobantes = new();
            foreach (var impuestoDto in impuestosDetalleDtos)
            {
                impuestoComprobantes.Add(new ImpuestoComprobante{
                    CodigoImpuesto = "2",
                    CodigoPorcentaje = impuestoDto.CodigoPorcentaje,
                    BaseImponible = impuestoDto.BaseImponible,
                    Valor = impuestoDto.ValorImpuesto,
                    Tarifa = impuestoDto.TarifaImpuesto
                });
            }
            return impuestoComprobantes;
        }

        private List<DatoAdicional> MapearDatosAdicionales(List<DatoAdicionalDto> datoAdicionalDtos)
        {
            List<DatoAdicional> datosAdicionales = new();
            foreach (var datoAdicionalDto in datoAdicionalDtos)
            {
                datosAdicionales.Add(new DatoAdicional
                {
                    Nombre = datoAdicionalDto.NombreAdicional,
                    Valor = datoAdicionalDto.ValorAdicional
                });
            }
            return datosAdicionales;
        }
    }
}