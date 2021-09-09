using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Repositories;
using ApiNetCore.Dtos;
using ApiNetCore.Entities;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/empresas")]
    [Authorize]
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresasRepository empresasRepository;

        public EmpresasController(IEmpresasRepository empresasRepository)
        {
            this.empresasRepository = empresasRepository;
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaCreadaDto>> PostAsync(EmpresaCreateDto empresaCreateDto)
        {
            try
            {
                 var empresaCreada = await empresasRepository.ObtenerEmpresaPorIdentificacion(empresaCreateDto.Ruc);

                if (empresaCreada != null)
                    return BadRequest(new EmpresaCreadaDto(
                        Ok: false,
                        Msg: "Empresa ya existe"
                    ));

                var empresa = new Empresa
                {
                    ObligadoContabilidad = empresaCreateDto.ObligadoContabilidad,
                    SecuencialNotaCredito = empresaCreateDto.SecuencialNotaCredito,
                    SecuencialRetencion = empresaCreateDto.SecuencialRetencion,
                    RegimenMicroempresas = empresaCreateDto.RegimenMicroempresas,
                    Activo = true,
                    Ambiente = empresaCreateDto.Ambiente,
                    TipoEmision = empresaCreateDto.TipoEmision,
                    RazonSocial = empresaCreateDto.RazonSocial,
                    NombreComercial = empresaCreateDto.NombreComercial,
                    Ruc = empresaCreateDto.Ruc,
                    Establecimiento = empresaCreateDto.Establecimiento,
                    PuntoEmision = empresaCreateDto.PuntoEmision,
                    DireccionMatriz = empresaCreateDto.DireccionMatriz,
                    DireccionEstablecimiento = empresaCreateDto.DireccionEstablecimiento,
                    ContribuyenteEspecial = empresaCreateDto.ContribuyenteEspecial,
                    SecuencialFactura = empresaCreateDto.SecuencialFactura,
                    ClaveFirma = empresaCreateDto.ClaveFirma,
                    PathCertificado = empresaCreateDto.PathCertificado,
                    MailEnvioComprobantes = empresaCreateDto.MailEnvioComprobantes,
                    PathLogo = empresaCreateDto.Ruc + ".jpg",
                    NombreNotificacio = empresaCreateDto.NombreNotificacion
                };

                await empresasRepository.CreateAsync(empresa);

                return Ok(new EmpresaCreadaDto(
                        Ok: true,
                        Msg: "Empresa Creada",
                        empresa
                    ));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new EmpresaCreadaDto(
                    Ok: false,
                    Msg: ex.Message
                ));
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<EmpresaRespuestaDto>> GetAsync()
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var nombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var empresas = (await empresasRepository.GetAllAsync())
                            .Select(empresa => empresa.AsDto());
            return Ok(new EmpresaRespuestaDto(
                Ok: true,
                Msg: uid + nombreUsuario,
                Empresas: empresas.ToList()
            ));
        }

        [HttpGet("configuracion/{id}")]
        public async Task<ActionResult<EmpresaConfiguracionDto>> ObtenerConfiguracionEmpresa(Guid id)
        {
            var empresa = await empresasRepository.GetAsync(id);
            return Ok(new EmpresaConfiguracionDto(
                Ok: true,
                Msg: "",
                SecuencialFactura: empresa.SecuencialFactura,
                SecuencialRetencion: empresa.SecuencialRetencion,
                SecuencialNotaCredito: empresa.SecuencialNotaCredito,
                PuntoEmision: empresa.PuntoEmision,
                Establecimiento: empresa.Establecimiento,
                RazonSocial: empresa.RazonSocial
            ));
        }
    }
}