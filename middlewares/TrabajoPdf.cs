using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiNetCore.Repositories;
using ApiNetCore.Services;
using HandlebarsDotNet;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace ApiNetCore.Middlewares
{
    public class TrabajoPdf
    {
        private readonly IBlobService blobService;

        private readonly IFacturaEmitidaRepository facturaEmitidaRepository;

        private readonly IClientesRepository clientesRepository;

        private readonly IConverter converter;
        public TrabajoPdf(IBlobService blobService, 
                            IFacturaEmitidaRepository facturaEmitidaRepository,
                            IClientesRepository clientesRepository,
                            IConverter converter
                        )
        {
            this.blobService = blobService;
            this.facturaEmitidaRepository = facturaEmitidaRepository;
            this.clientesRepository = clientesRepository;
            this.converter = converter;
        }

        public async Task<string> ObtenerTemplate(string tipoDocumento)
        {
            var blobClient = blobService.GetBlob($"{tipoDocumento}.html", "templates");
            string template = String.Empty;

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                using (var streamReader= new StreamReader(response.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        template += await streamReader.ReadLineAsync();
                    }
                }
            }
            return template;
        }

        private async Task<object> ObtenerDatos(string claveAcceso)
        {
            List<object> detalles = new();
            List<object> adicionales = new();
            decimal subTotalCero = 0.00m, subTotalDoce = 0.00m, subTotalNoImpuesto = 0.00m;
            var facturaEmitida = await facturaEmitidaRepository.GetPorClaveAcceso(claveAcceso);
            var cliente = await clientesRepository.GetAsync(Guid.Parse(facturaEmitida.IdCliente));
            foreach (var impuesto in facturaEmitida.ImpuestosComprobante)
            {
                var baseImponible = Convert.ToDecimal(impuesto.BaseImponible);
                if (impuesto.CodigoPorcentaje == "2")
                    subTotalDoce += baseImponible;
                else if (impuesto.CodigoPorcentaje == "0")
                    subTotalCero += baseImponible;
                else
                    subTotalNoImpuesto += baseImponible;
            }
            var informacion = new {
                numeroFactura = $"{facturaEmitida.Estab}-{facturaEmitida.PtoEmi}-{facturaEmitida.Secuencial}",
                fecha = facturaEmitida.FechaEmision,
                razonSocial = facturaEmitida.RazonSocial,
                ruc = facturaEmitida.Ruc,
                nombreComercial = facturaEmitida.NombreComercial,
                direccionEstablecimiento = facturaEmitida.DirEstablecimiento,
                contribuyenteEspecial = facturaEmitida.ContribuyenteEspecial,
                obligadoContabilidad = facturaEmitida.ObligadoContabilidad,
                regimenMicroempresas = facturaEmitida.RegimenMicroempresas,
                claveAcceso = facturaEmitida.ClaveAcceso,
                ambiente = facturaEmitida.Ambiente,
                tipoEmision = facturaEmitida.TipoEmision,
                razonCliente = facturaEmitida.RazonSocialComprador,
                tipoIdentificacionCliente = facturaEmitida.TipoIdentificacionComprador,
                identificacionCliente = facturaEmitida.IdentificacionComprador,
                direccionCliente = cliente.Direccion,
                telefonoCliente = cliente.Telefono,
                emailCliente = cliente.Mail,
                subTotalDoce,
                subTotalCero,
                subTotalNoImpuesto,
                subTotalSinImpuesto = Convert.ToDecimal(string.Format("{0:.00}", facturaEmitida.TotalSinImpuestos)),
                valorDescuento = Convert.ToDecimal(string.Format("{0:.00}", facturaEmitida.TotalDescuento)),
                valorIva = Convert.ToDecimal(string.Format("{0:.00}", facturaEmitida.TotalIva)),
                valorTotal = Convert.ToDecimal(string.Format("{0:.00}", facturaEmitida.ImporteTotal))
            };
            foreach (var detalle in facturaEmitida.Detalles)
            {
                detalles.Add(new {
                    codigoPrincipal = detalle.CodigoPrincipal,
                    descripcion = detalle.Descripcion,
                    cantidad = detalle.Cantidad,
                    precioUnitario = Convert.ToDecimal(string.Format("{0:.00}", detalle.PrecioUnitario)),
                    descuento = Convert.ToDecimal(string.Format("{0:.00}", detalle.Descuento)),
                    totalSinImpuesto = Convert.ToDecimal(string.Format("{0:.00}", detalle.TotalSinImpuesto))
                });
            }
            foreach (var adicional in facturaEmitida.DatosAdicionales)
            {
                adicionales.Add(new {
                    nombreAdicional = adicional.Nombre,
                    valorAdicional = adicional.Valor
                });
            }
            
            var data = new {
                informacion,
                detalles,
                adicionales,
                formasPago = new []
                {
                    new {
                        formaPago = facturaEmitida.FormaPago.Codigo,
                        total = facturaEmitida.FormaPago.Valor
                    }
                }
            };
            return data;
        }

        public async Task<byte[]> CreateDocument(string claveAcceso)
        {
            var htmlContent = await ObtenerTemplate("Factura");

            var logo = $"https://imageneschatecuador.s3.us-east-2.amazonaws.com/{claveAcceso.Substring(10,13)}.png";

            htmlContent = string.Join(claveAcceso,htmlContent.Split("claveBarcode"));
            htmlContent = string.Join(logo,htmlContent.Split("logo.png"));

            var template = Handlebars.Compile(htmlContent);

            var datos = await ObtenerDatos(claveAcceso);

            var resultado = template(datos);
            
            var archivo = converter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = resultado,
                        WebSettings = { DefaultEncoding = "utf-8" },
                    },
                },
            });

            await blobService.UploadBlob($"{claveAcceso}.pdf", "pdf", archivo);

            return archivo;
        }
    }
}