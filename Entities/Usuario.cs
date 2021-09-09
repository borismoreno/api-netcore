using System;

namespace ApiNetCore.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string Rol { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Activo { get; set; }

        public string IdEmpresa { get; set; }
    }
}