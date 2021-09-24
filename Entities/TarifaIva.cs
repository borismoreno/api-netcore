using System;
namespace ApiNetCore.Entities
{
    public class TarifaIva
    {
        public Guid Id { get; set; }

        public bool Activo { get; set; }

        public string Porcentaje { get; set; }

        public string Codigo { get; set; }

        public decimal ValorPorcentaje { get; set; }
    }
}