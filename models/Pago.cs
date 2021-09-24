using System.Xml.Serialization;

namespace ApiNetCore.Models
{
    public class Pago
    {
        public string FormaPago { get; set; }

        public decimal Total { get; set; }

        public int Plazo { get; set; }

        public string UnidadTiempo { get; set; }

        [XmlIgnore]
        public string DescripcionFormaPago { get; set; }
    }
}