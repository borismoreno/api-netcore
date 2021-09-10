using ApiNetCore.Repositories;
using ApiNetCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiNetCore.Dtos;
using System.Security.Claims;
using System.Linq;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesRepository clientesRepository;

        public ClientesController(IClientesRepository clientesRepository)
        {
            this.clientesRepository = clientesRepository;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ClienteInsertarDto clienteInsertarDto)
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var cliente = new Cliente
            {
                Activo = true,
                RazonSocial = clienteInsertarDto.RazonSocial,
                TipoIdentificacion = clienteInsertarDto.TipoIdentificacion,
                NumeroIdentificacion = clienteInsertarDto.NumeroIdentificacion,
                Telefono = clienteInsertarDto.Telefono,
                Mail = clienteInsertarDto.Mail,
                Direccion = clienteInsertarDto.Direccion,
                Usuario = uid
            };
            await clientesRepository.CreateAsync(cliente);
            return Ok(new ClienteInsertadoDto(
                Ok: true,
                Msg: "",
                Cliente: cliente
            ));
        }

        [HttpGet]
        public async Task<ActionResult<ClientesConsultadoDto>> GetAsync()
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var clientes = await clientesRepository.GetClientesAsync(uid);
            return Ok(new ClientesConsultadoDto
            (
                Ok: true,
                Msg: "",
                Clientes: clientes.ToList()
            ));
        }
    }
}