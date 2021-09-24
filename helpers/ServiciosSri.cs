using ApiNetCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace ApiNetCore.Helpers
{
    public class ServiciosSri
    {
        public RespuestaSri EnviarComprobante(byte[] xmlComprobante, string urlEnvio)
        {
            RespuestaSri respuestaSri = null;
            try
            {
                 string bytesEncoded = ConvertToBase64String(xmlComprobante);
                 string cadenaComprobante = string.Concat("<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ec='http://ec.gob.sri.ws.recepcion'><soapenv:Header/><soapenv:Body><ec:validarComprobante><xml>", bytesEncoded, "</xml></ec:validarComprobante></soapenv:Body></soapenv:Envelope>");

                WebRequest length = WebRequest.Create(urlEnvio);
                length.Method = "POST";
                byte[] bytes = Encoding.UTF8.GetBytes(cadenaComprobante);
                length.ContentType = "text/xml; charset=UTF-8";
                length.Timeout = 50000;
                length.ContentLength = (long)(checked((int)bytes.Length));
                Stream requestStream = length.GetRequestStream();
                requestStream.Write(bytes, 0, checked((int)bytes.Length));
                requestStream.Close();
                WebResponse response = length.GetResponse();
                requestStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(requestStream);
                string end = streamReader.ReadToEnd();
                streamReader.Close();
                requestStream.Close();
                response.Close();
                respuestaSri = ObtenerRespuestaRecepcion(end);
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return respuestaSri;
        }

        private static string ConvertToBase64String(byte[] xmlComprobante)
        {
            return Convert.ToBase64String(xmlComprobante);
        }

        private static RespuestaSri ObtenerRespuestaRecepcion(string xmlRespuesta)
        {
            RespuestaSri respuestaSri = new();
            respuestaSri.XmlRespuesta = xmlRespuesta;
            XmlDocument xDoc = new();
            xDoc.LoadXml(xmlRespuesta);

            XmlNodeList nodoEstado = xDoc.GetElementsByTagName("estado");

            if(nodoEstado.Count > 0)
                respuestaSri.Estado = nodoEstado[0].InnerText;
            return respuestaSri;
        }

        public EstadoComprobante RecuperarEstadoComprobante(string estado)
        {
            EstadoComprobante estadoComprobante = EstadoComprobante.Indeterminado;
            List<EstadoComprobante> estados = Enum.GetValues(typeof(EstadoComprobante)).Cast<EstadoComprobante>().ToList();

            foreach (var itemEstado in estados)
            {
                if (estado == EnumString.GetStringValue(itemEstado))
                {
                    estadoComprobante = itemEstado;
                    break;
                }
            }
            return estadoComprobante;
        }
    }
}