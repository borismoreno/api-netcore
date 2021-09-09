using ApiNetCore.Repositories;
using ApiNetCore.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/productos")]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        private readonly IProductosRepository productosRepository;

        public ProductosController(IProductosRepository productosRepository)
        {
            this.productosRepository = productosRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ProductoInsertarDto productoInsertarDto)
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var producto = new Producto
            {
                CodigoPrincipal = productoInsertarDto.CodigoPrincipal,
                CodigoAuxiliar = productoInsertarDto.CodigoAuxiliar,
                Activo = true,
                TipoProducto = productoInsertarDto.TipoProducto,
                TarifaIva = productoInsertarDto.TarifaIva,
                Descripcion = productoInsertarDto.Descripcion,
                ValorUnitario = 56.56m,
                Usuario = uid
            };
            await productosRepository.CreateAsync(producto);
            return Ok(new ProductoInsertadoDto(
                Ok: true,
                Msg: "",
                Producto: producto
            ));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductosConsultaDto>>> GetAsync()
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var productos = await productosRepository.GetProductosAsync(uid);
            return Ok(new ProductosConsultaDto(
                Ok: true,
                Msg: "",
                Productos: productos.ToList()
            ));
        }
    }
}