using System.Collections.Generic;

namespace ApiNetCore.Models
{
    public class Detalle
    {
        public string CodigoPrincipal { get; set; }

        public string CodigoAuxiliar { get; set; }

        public string Descripcion { get; set; }

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal PrecioTotalSinImpuesto { get; set; }

        public List<Impuesto> Impuestos { get; set; }

        public Detalle()
        {
            Impuestos = new();
        }
    }
}