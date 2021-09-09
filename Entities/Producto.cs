using System;

namespace ApiNetCore.Entities
{
    public class Producto
    {
        public Guid Id { get; set; }

        public string CodigoPrincipal { get; set; }

        public string CodigoAuxiliar { get; set; }

        public bool Activo { get; set; }

        public string TipoProducto { get; set; }

        public string TarifaIva { get; set; }

        public string Descripcion { get; set; }

        public decimal ValorUnitario { get; set; }

        public string Usuario { get; set; }

    }
}