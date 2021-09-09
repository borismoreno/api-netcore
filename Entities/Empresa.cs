using System;
using System.ComponentModel.DataAnnotations;

namespace ApiNetCore.Entities
{
    public class Empresa
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ObligadoContabilidad { get; set; }

        [Required]
        public string SecuencialNotaCredito { get; set; }

        [Required]
        public string SecuencialRetencion { get; set; }

        [Required]
        public bool RegimenMicroempresas { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public int Ambiente { get; set; }

        [Required]
        public int TipoEmision { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public string NombreComercial { get; set; }

        [Required]
        public string Ruc { get; set; }

        [Required]
        public string Establecimiento { get; set; }

        [Required]
        public string PuntoEmision { get; set; }

        [Required]
        public string DireccionMatriz { get; set; }

        [Required]
        public string DireccionEstablecimiento { get; set; }

        [Required]
        public string ContribuyenteEspecial { get; set; }

        [Required]
        public string SecuencialFactura { get; set; }

        [Required]
        public string ClaveFirma { get; set; }

        [Required]
        public string PathCertificado { get; set; }

        [Required]
        public string MailEnvioComprobantes { get; set; }

        [Required]
        public string PathLogo { get; set; }

        [Required]
        public string NombreNotificacio { get; set; }

        
    }
}