using System;

namespace ApiNetCore.Entities
{
    public class TipoProducto
    {
        public Guid Id { get; set; }

        public bool Activo { get; set; }

        public string Descripcion { get; set; }

        public string Codigo { get; set; }

        public DateTimeOffset FechaModificacion { get; set; }

    }
}