using System;
using System.Collections.Generic;

namespace ApiNetCore.Entities
{
    public class FacturaEmitida
    {
        public Guid Id { get; set; }
        
        public string ContribuyenteEspecial { get; set; }

        public bool RegimenMicroempresas { get; set; }

        public int Ambiente { get; set; }

        public int TipoEmision { get; set; }

        public string RazonSocial { get; set; }

        public string NombreComercial { get; set; }

        public string Ruc { get; set; }

        public string ClaveAcceso { get; set; }

        public string CodDoc { get; set; }

        public string Estab { get; set; }

        public string PtoEmi { get; set; }

        public string Secuencial { get; set; }

        public string DirMatriz { get; set; }

        public string FechaEmision { get; set; }

        public string DirEstablecimiento { get; set; }

        public string ObligadoContabilidad { get; set; }

        public string TipoIdentificacionComprador { get; set; }

        public string RazonSocialComprador { get; set; }

        public string IdentificacionComprador { get; set; }

        public decimal TotalSinImpuestos { get; set; }

        public decimal TotalDescuento { get; set; }

        public decimal Propina { get; set; }

        public decimal TotalIva { get; set; }

        public decimal ImporteTotal { get; set; }

        public string Moneda { get; set; }

        public string EstadoComprobante { get; set; }

        public DateTimeOffset FechaRegistro { get; set; }

        public string IdUsuario { get; set; }

        public string IdCliente { get; set; }

        public string PathPdf { get; set; }

        public string PathXml { get; set; }

        public FormaPago FormaPago { get; set; }

        public List<ImpuestoComprobante> ImpuestosComprobante { get; set; }

        public List<Detalle> Detalles { get; set; }

        public List<DatoAdicional> DatosAdicionales { get; set; }
    }

    public class FormaPago
    {
        public string Codigo { get; set; }

        public decimal Valor { get; set; }

        public int Plazo { get; set; }

        public string UnidadTiempo { get; set; }
    }

    public class ImpuestoComprobante
    {
        public string CodigoImpuesto { get; set; }

        public string CodigoPorcentaje { get; set; }

        public decimal BaseImponible { get; set; }

        public decimal Valor { get; set; }

        public decimal Tarifa { get; set; }
    }

    public class Detalle
    {
        public string CodigoPrincipal { get; set; }

        public string Descripcion { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal TotalSinImpuesto { get; set; }

        public decimal ValorImpuesto { get; set; }

        public ImpuestoComprobante ImpuestoDetalle { get; set; }
    }

    public class DatoAdicional
    {
        public string Nombre { get; set; }

        public string Valor { get; set; }
    }
}