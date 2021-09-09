using System;

namespace ApiNetCore.Entities
{
    public class TipoFormaPago
    {
        public Guid Id { get; set; }

        public bool Activo { get; set; }

        public string FormaPago { get; set; }

        public string Codigo { get; set; }

        public DateTimeOffset FechaModificacion { get; set; }

        public string Usuario { get; set; }

        public string[] TipoPago { get; set; }
    }
}