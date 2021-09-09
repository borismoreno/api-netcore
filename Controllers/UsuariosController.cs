using Microsoft.AspNetCore.Mvc;
using ApiNetCore.Repositories;
using System.Threading.Tasks;
using ApiNetCore.Dtos;
using ApiNetCore.Entities;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;
using System;
using ApiNetCore.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository usuariosRepository;
        private readonly IEmpresasRepository empresasRepository;
        private readonly IConfiguration configuration;

        public UsuariosController(IUsuariosRepository usuariosRepository, IConfiguration configuration, IEmpresasRepository empresasRepository)
        {
            this.usuariosRepository = usuariosRepository;
            this.configuration = configuration;
            this.empresasRepository = empresasRepository;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioCreadoDto>> PostAsync(UsuarioCrearDto usuarioCrearDto)
        {
            try
            {
                 var usuario = new Usuario
                 {
                     Rol = usuarioCrearDto.Rol,
                     Nombre = usuarioCrearDto.Nombre,
                     Email = usuarioCrearDto.Email,
                     Password = BCryptNet.HashPassword(usuarioCrearDto.Password, 10),
                     Activo = true,
                     IdEmpresa = usuarioCrearDto.IdEmpresa
                 };

                 await usuariosRepository.CreateAsync(usuario);
                 return Ok(new UsuarioCreadoDto(
                     Ok: true,
                     Msg: "Usuario creado",
                     Id: usuario.Id
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioAutenticadoDto>> LoginAsync(LoginDto loginDto)
        {
            var usuario = await usuariosRepository.ObtenerUsuarioPorLoginAsync(loginDto.Email);
            if (usuario == null)
                return BadRequest(new UsuarioAutenticadoDto(
                    Ok: false,
                    Msg: "[Usuario] o password incorrecto"
                ));
            var verificado = BCryptNet.Verify(loginDto.Password, usuario.Password);
            if (!verificado)
            {
                return BadRequest(new UsuarioAutenticadoDto(
                    Ok: false,
                    Msg: "Usuario o [password] incorrecto"
                ));
            }

            var jwt = new jwt(configuration);
            var token = jwt.GenerarJWT(usuario.Id.ToString(), usuario.Nombre);

            var empresa = await empresasRepository.GetAsync(Guid.Parse(usuario.IdEmpresa));
            return Ok(new UsuarioAutenticadoDto(
                Ok: true,
                Msg: "Autenticado",
                Uid: usuario.Id.ToString(),
                Nombre: usuario.Nombre,
                NombreComercial: empresa.NombreComercial,
                EmpresaId: usuario.IdEmpresa,
                Token: token
            ));
        }

        [HttpGet("renew")]
        public async Task<ActionResult<UsuarioAutenticadoDto>> RevalidarToken()
        {
            var uid = ((ClaimsIdentity)User.Identity).FindFirst("uid").Value;
            var nombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = await usuariosRepository.ObtenerPorIdAsync(Guid.Parse(uid));
            var empresa = await empresasRepository.GetAsync(Guid.Parse(usuario.IdEmpresa));
            var jwt = new jwt(configuration);
            var token = jwt.GenerarJWT(usuario.Id.ToString(), usuario.Nombre);
            return Ok(new UsuarioAutenticadoDto(
                Ok: true,
                Msg: "Autenticado",
                Uid: usuario.Id.ToString(),
                Nombre: usuario.Nombre,
                NombreComercial: empresa.NombreComercial,
                EmpresaId: usuario.IdEmpresa,
                Token: token
            ));
        }
    }
}