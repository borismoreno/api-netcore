namespace ApiNetCore.Models
{
    public class TotalImpuesto
    {
        public string Codigo { get; set; }

        public int CodigoPorcentaje { get; set; }

        public decimal BaseImponible { get; set; }

        public decimal Valor { get; set; }

        public TotalImpuesto()
        {
            Codigo = "";
            CodigoPorcentaje = 0;
            BaseImponible = 0.00m;
            Valor = 0.00m;
        }
    }
}