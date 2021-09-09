using ApiNetCore.Repositories;
using ApiNetCore.Dtos;
using ApiNetCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/configuracion")]
    [Authorize]
    public class ConfiguracionController : ControllerBase
    {
        private readonly ITiposFormaPagoRepository tiposFormaPagoRepository;
        private readonly ITipoProductoRepository tipoProductoRepository;
        private readonly ITarifasIvaRepository tarifasIvaRepository;
        public ConfiguracionController(ITiposFormaPagoRepository tiposFormaPagoRepository, ITipoProductoRepository tipoProductoRepository, ITarifasIvaRepository tarifasIvaRepository)
        {
            this.tiposFormaPagoRepository = tiposFormaPagoRepository;
            this.tipoProductoRepository = tipoProductoRepository;
            this.tarifasIvaRepository = tarifasIvaRepository;
        }

        [HttpPost("tipoFormaPago")]
        public async Task<ActionResult> PostAsync(TipoFormaPagoInsertDto tipoFormaPagoInsertDto)
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var tipoFormaPago = new TipoFormaPago
            {
                Activo = true,
                FormaPago = tipoFormaPagoInsertDto.FormaPago,
                Codigo = tipoFormaPagoInsertDto.Codigo,
                TipoPago = tipoFormaPagoInsertDto.TipoPago,
                FechaModificacion = DateTimeOffset.UtcNow,
                Usuario = uid
            };
            await tiposFormaPagoRepository.CreateAsync(tipoFormaPago);
            return Ok();
        }

        [HttpGet("tiposFormaPago")]
        public async Task<ActionResult<IEnumerable<TiposFormaPagoDto>>> GetTiposFormaPagoAsync()
        {
            var tiposFormaPago = await tiposFormaPagoRepository.GetAsync();
            return Ok(new TiposFormaPagoDto(
                Ok: true,
                Msg: "",
                TiposFormaPago: tiposFormaPago.ToList()
            ));
        }

        [HttpPost("tipoProducto")]
        public async Task<ActionResult> PostAsync(TipoProductoInsertarDto tipoProductoInsertarDto)
        {
            var tipoProducto = new TipoProducto
            {
                Activo = true,
                Descripcion = tipoProductoInsertarDto.Descripcion,
                Codigo = tipoProductoInsertarDto.Codigo,
                FechaModificacion = DateTimeOffset.UtcNow
            };
            await tipoProductoRepository.CreateAsync(tipoProducto);
            return Ok();
        }

        [HttpGet("tiposProducto")]
        public async Task<ActionResult<IEnumerable<TiposProductoDto>>> GetTiposProductoAsync()
        {
            var tiposProducto = await tipoProductoRepository.GetAsync();
            return Ok(new TiposProductoDto(
                Ok: true,
                Msg: "",
                TiposProducto: tiposProducto.ToList()
            ));
        }

        [HttpPost("tarifaIva")]
        public async Task<ActionResult> PostAsync(TarifaIvaInsertarDto tarifaIvaInsertarDto)
        {
            var tarifaIva = new TarifaIva
            {
                Activo = true,
                Porcentaje = tarifaIvaInsertarDto.Porcentaje,
                Codigo = tarifaIvaInsertarDto.Codigo,
            };
            await tarifasIvaRepository.CreateAsync(tarifaIva);
            return Ok();
        }

        [HttpGet("tarifasIva")]
        public async Task<ActionResult<IEnumerable<TarifasIvaConsultaDto>>> GetTarifasIvaAsync()
        {
            var tarifasIva = await tarifasIvaRepository.GetAsync();
            return Ok(new TarifasIvaConsultaDto(
                Ok: true,
                Msg: "",
                TarifasIva: tarifasIva.ToList()
            ));
        }

    }
}