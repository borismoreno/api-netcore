using System.Xml.Serialization;
using System.Collections.Generic;

namespace ApiNetCore.Models
{
    public class Factura : Documento
    {
        public InformacionFactura InfoFactura { get; set; }

        public List<Detalle> Detalles { get; set; }

        public List<string> InfoAdicional { get; set; }

        public Factura()
        {
            InfoFactura = new();
            Detalles = new();
            InfoAdicional = new();
        }
    }

    public class InformacionFactura
    {
        public string FechaEmision { get; set; }

        public string DirEstablecimiento { get; set; }

        public int? ContribuyenteEspecial { get; set; }

        [XmlIgnore]
        public bool ContribuyenteEspecialSpecified { get { return ContribuyenteEspecial >= 0; } }

        public string ObligadoContabilidad { get; set; }

        public string TipoIdentificacionComprador { get; set; }

        public string RazonSocialComprador { get; set; }

        public string IdentificacionComprador { get; set; }

        public decimal TotalSinImpuestos { get; set; }

        public decimal TotalDescuento { get; set; }

        public List<TotalImpuesto> TotalConImpuestos { get; set; }

        public decimal Propina { get; set; }

        public decimal ImporteTotal { get; set; }

        public string Moneda { get; set; }

        public List<Pago> Pagos { get; set; }

        public InformacionFactura()
        {
            TotalConImpuestos = new();
            Pagos = new();
        }
    }
}