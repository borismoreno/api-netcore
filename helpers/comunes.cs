using System;
using ApiNetCore.Entities;

namespace ApiNetCore.Helpers
{
    public class Comunes
    {
        public string invertirCadena(string cadena)
        {
            var cadenaInvertida = "";
            for (int i = cadena.Length - 1; i >= 0; i--)
            {
                cadenaInvertida = cadenaInvertida + cadena[i];
            }
            return cadenaInvertida;
        }

        public int obtenerSumaPorDigitos(string cadena)
        {
            var pivote = 2;
            var longitudCadena = cadena.Length;
            var cantidadTotal = 0;
            for (int i = 0; i < longitudCadena; i++)
            {
                if (pivote == 8)
                    pivote = 2;
                var temporal = int.Parse(cadena.Substring(i, 1));
                temporal *= pivote;
                pivote++;
                cantidadTotal += temporal;
            }
            cantidadTotal = 11 - cantidadTotal % 11;
            if (cantidadTotal == 11)
                cantidadTotal = 0;
            else if (cantidadTotal == 10)
                cantidadTotal = 1;
            return cantidadTotal;
        }

        public string obtenerNumeroAleatorio()
        {
            var aleatorio = "";
            var random = new Random();
            var aux = random.Next(1000);
            aleatorio = aux.ToString().PadLeft(8,'0');
            return aleatorio;
        }

        public string generarClaveAcceso(string fecha, string codigoDocumento, Empresa empresa)
        {
            var secuencial = "";
            switch (codigoDocumento)
            {
                case "01":
                    secuencial = empresa.SecuencialFactura;
                    break;
                default:
                    break;
            }
            var claveAcceso = fecha + codigoDocumento + empresa.Ruc + empresa.Ambiente + empresa.Establecimiento
                                + empresa.PuntoEmision + secuencial.PadLeft(9,'0') + obtenerNumeroAleatorio() + empresa.TipoEmision;
            claveAcceso = claveAcceso + obtenerSumaPorDigitos(invertirCadena(claveAcceso));
            
            return claveAcceso;
        }
    }
}