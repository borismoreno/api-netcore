using System;

namespace ApiNetCore.Entities
{
    public class TipoIdentificacion
    {
        public Guid Id { get; set; }

        public bool Activo { get; set; }

        public string Descripcion { get; set; }

        public string Codigo { get; set; }
    }
}