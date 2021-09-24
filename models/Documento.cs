namespace ApiNetCore.Models
{
    public class Documento
    {
        public InformacionTributaria InfoTributaria { get; set; }

        public Documento()
        {
            InfoTributaria = new();
        }
    }

    public class InformacionTributaria
    {
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

        public string RegimenMicroempresas { get; set; }
    }
}