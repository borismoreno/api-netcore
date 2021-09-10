using System;

namespace ApiNetCore.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }

        public string RazonSocial { get; set; }

        public bool Activo { get; set; }

        public string TipoIdentificacion { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string Telefono { get; set; }

        public string Mail { get; set; }

        public string Direccion { get; set; }

        public string Usuario { get; set; }
    }
}