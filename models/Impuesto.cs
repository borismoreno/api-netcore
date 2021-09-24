namespace ApiNetCore.Models
{
    public class Impuesto
    {
        public string Codigo { get; set; }

        public int CodigoPorcentaje { get; set; }

        public decimal Tarifa { get; set; }

        public decimal BaseImponible { get; set; }

        public decimal Valor { get; set; }

        public Impuesto()
        {
            Codigo = "";
            CodigoPorcentaje = 0;
            Tarifa = 0;
            BaseImponible = 0;
            Valor = 0;
        }
    }
}