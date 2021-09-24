using ApiNetCore.Entities;
using ApiNetCore.Models;
using es.mityc.firmaJava.libreria.utilidades;
using es.mityc.firmaJava.libreria.xades;
using es.mityc.javasign.pkstore;
using es.mityc.javasign.pkstore.keystore;
using es.mityc.javasign.trust;
using es.mityc.javasign.xml.xades.policy;
using es.mityc.javasign.xml.xades.policy.facturae;
using java.io;
using java.security;
using java.security.cert;
using javax.xml.parsers;
using org.w3c.dom;
using sviudes.blogspot.com;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using ApiNetCore.Services;
using System.Threading.Tasks;

namespace ApiNetCore.Middlewares
{
    public class TrabajoXml
    {
        private readonly IBlobService blobService;

        public TrabajoXml(IBlobService blobService)
        {
            this.blobService = blobService;
        }
        public byte[] GenerarXmlComprobante(FacturaEmitida facturaEmitida, string pathCertificado, string password)
        {
            object objetoComprobante = MapearFacturaModel(facturaEmitida);
            var xmlDoc = ConvertObjectToXml(objetoComprobante);
            xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            xmlDoc.InnerXml = Depurar(xmlDoc.InnerXml);

            var bytesXml = ConvertXmlDocumentToBytes(xmlDoc);

            bytesXml = GenerarFirma(bytesXml, pathCertificado, password);
            return bytesXml;
        }

        private XmlDocument ConvertObjectToXml(object objeto)
        {
            XmlDocument xmlDocument = new();
            using (MemoryStream xmlStream = new())
            {
                 var xmlSerializer = new XmlSerializer(objeto.GetType());
                 xmlSerializer.Serialize(xmlStream, objeto);
                 xmlStream.Position = 0;
                 xmlDocument.Load(xmlStream);
            }
            return xmlDocument;
        }

        private string Depurar(string xmlGenerado)
        {
            if (xmlGenerado.Contains("<Factura "))
            {
                xmlGenerado = xmlGenerado.Replace("<Factura ", "<Factura id = \"" + "comprobante\"" + " version=\"" + "1.1.0\"" + " ");
            }
            if (xmlGenerado.Contains("<?xml version=\"" + "1.0\""))
            {
                xmlGenerado = xmlGenerado.Replace("<?xml version=\"" + "1.0\"", "<?xml version=\"" + "1.0\"" + " encoding=\"" + "UTF-8\"" + " ");
            }
            if (xmlGenerado.Contains("<string>"))
            {
                xmlGenerado = xmlGenerado.Replace("<string>","");
                xmlGenerado = xmlGenerado.Replace("</string>","");
            }
            if (xmlGenerado.Contains("&gt;"))
            {
                xmlGenerado = xmlGenerado.Replace("&gt;",">");
            }
            if (xmlGenerado.Contains("&lt;"))
            {
                xmlGenerado = xmlGenerado.Replace("&lt;","<");
            }
            string cadenaAEvaluar = "";
            string cadenaAEvaluar2 = "";

            for (int i = 65; i < 90; i++)
            {
                cadenaAEvaluar = "<" + (char)i;
                cadenaAEvaluar2 = "</" + (char)i;
                if (xmlGenerado.Contains(cadenaAEvaluar))
                {
                    xmlGenerado = xmlGenerado.Replace(cadenaAEvaluar, "<" + Char.ToLower((char)i));
                }
                if (xmlGenerado.Contains(cadenaAEvaluar2))
                {
                    xmlGenerado = xmlGenerado.Replace(cadenaAEvaluar2, "</" + Char.ToLower((char)i));
                }
            }
            return xmlGenerado;
        }

        private Factura MapearFacturaModel(FacturaEmitida facturaEmitida)
        {
            Factura factura = new();
            factura.InfoTributaria.Ambiente = facturaEmitida.Ambiente;
            factura.InfoTributaria.TipoEmision = facturaEmitida.TipoEmision;
            factura.InfoTributaria.RazonSocial = facturaEmitida.RazonSocial;
            factura.InfoTributaria.NombreComercial = facturaEmitida.NombreComercial;
            factura.InfoTributaria.Ruc = facturaEmitida.Ruc;
            factura.InfoTributaria.ClaveAcceso = facturaEmitida.ClaveAcceso;
            factura.InfoTributaria.CodDoc = facturaEmitida.CodDoc;
            factura.InfoTributaria.Estab = facturaEmitida.Estab;
            factura.InfoTributaria.PtoEmi = facturaEmitida.PtoEmi;
            factura.InfoTributaria.Secuencial = facturaEmitida.Secuencial;
            factura.InfoTributaria.DirMatriz = facturaEmitida.DirMatriz;

            if (facturaEmitida.RegimenMicroempresas)
                factura.InfoTributaria.RegimenMicroempresas = "CONTRIBUYENTE RÉGIMEN MICROEMPRESAS";
            else
                factura.InfoTributaria.RegimenMicroempresas = null;

            factura.InfoFactura.FechaEmision = facturaEmitida.FechaEmision;
            factura.InfoFactura.DirEstablecimiento = facturaEmitida.DirEstablecimiento;
            factura.InfoFactura.ObligadoContabilidad = facturaEmitida.ObligadoContabilidad;
            factura.InfoFactura.TipoIdentificacionComprador = facturaEmitida.TipoIdentificacionComprador;
            factura.InfoFactura.RazonSocialComprador = facturaEmitida.RazonSocialComprador;
            factura.InfoFactura.IdentificacionComprador = facturaEmitida.IdentificacionComprador;
            factura.InfoFactura.TotalSinImpuestos = facturaEmitida.TotalSinImpuestos;
            factura.InfoFactura.TotalDescuento = facturaEmitida.TotalDescuento;
            factura.InfoFactura.ImporteTotal = facturaEmitida.ImporteTotal;
            factura.InfoFactura.Propina = facturaEmitida.Propina;
            factura.InfoFactura.Moneda = facturaEmitida.Moneda;

            if (facturaEmitida.ContribuyenteEspecial != "")
                factura.InfoFactura.ContribuyenteEspecial = Convert.ToInt32(facturaEmitida.ContribuyenteEspecial);
            else
                factura.InfoFactura.ContribuyenteEspecial = null;

            var listaPorcentajesImpuestos = facturaEmitida.ImpuestosComprobante.Select(x => x.CodigoPorcentaje).Distinct().ToList();
            foreach (var codigoPorcentaje in listaPorcentajesImpuestos)
            {
                TotalImpuesto totalImpuesto = new();
                var listaImpuestosPorcentaje = facturaEmitida.ImpuestosComprobante.FindAll(x => x.CodigoPorcentaje == codigoPorcentaje);
                var datosImpuestosPorcentaje = listaImpuestosPorcentaje.FirstOrDefault();

                totalImpuesto.Codigo = datosImpuestosPorcentaje.CodigoImpuesto;
                totalImpuesto.CodigoPorcentaje = Convert.ToInt32(datosImpuestosPorcentaje.CodigoPorcentaje);
                totalImpuesto.Valor = Math.Round(listaImpuestosPorcentaje.Sum(x => x.Valor), 2);
                totalImpuesto.BaseImponible = Math.Round(listaImpuestosPorcentaje.Sum(x => x.BaseImponible), 2);

                factura.InfoFactura.TotalConImpuestos.Add(totalImpuesto);
            }

            foreach (var detalle in facturaEmitida.Detalles)
            {
                Models.Detalle detalleDocumento = new();
                detalleDocumento.CodigoPrincipal = detalle.CodigoPrincipal;
                detalleDocumento.Descripcion = detalle.Descripcion;
                detalleDocumento.Cantidad = detalle.Cantidad;
                detalleDocumento.PrecioUnitario = detalle.PrecioUnitario;
                detalleDocumento.Descuento = detalle.Descuento;
                detalleDocumento.PrecioTotalSinImpuesto = detalle.TotalSinImpuesto;

                Impuesto impuestoGenerado = new();
                impuestoGenerado.Codigo = detalle.ImpuestoDetalle.CodigoImpuesto;
                impuestoGenerado.CodigoPorcentaje = Convert.ToInt32(detalle.ImpuestoDetalle.CodigoPorcentaje);
                impuestoGenerado.Tarifa = detalle.ImpuestoDetalle.Tarifa;
                impuestoGenerado.Valor = detalle.ImpuestoDetalle.Valor;
                impuestoGenerado.BaseImponible = detalle.ImpuestoDetalle.BaseImponible;

                detalleDocumento.Impuestos.Add(impuestoGenerado);
                factura.Detalles.Add(detalleDocumento);
            }

            foreach (var campoAdicional in facturaEmitida.DatosAdicionales)
            {
                factura.InfoAdicional.Add("\r<campoAdicional nombre= \"" + campoAdicional.Nombre + "\">" + campoAdicional.Valor + "</campoAdicional>\r");
            }

            factura.InfoFactura.Pagos.Add(new Pago{
                FormaPago = facturaEmitida.FormaPago.Codigo,
                Total = facturaEmitida.FormaPago.Valor,
                Plazo = facturaEmitida.FormaPago.Plazo,
                UnidadTiempo = facturaEmitida.FormaPago.UnidadTiempo
            });
            return factura;
        }

        private byte[] ConvertXmlDocumentToBytes(XmlDocument xmlDocument)
        {
            return Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
        }

        private byte[] GenerarFirma(byte[] archivoXml, string pathCertificado, string password)
        {
            byte[] firmado = null;
            PrivateKey privateKey = null;
            Provider provider = null;

            try
            {
                X509Certificate x509Certificate = LoadCertificate(pathCertificado, password, ref privateKey, ref provider);
                TrustFactory.instance = TrustFactory.newInstance();
                TrustFactory.truster = PropsTruster.getInstance();
                PoliciesManager.POLICY_SIGN = new Facturae31Manager();
                PoliciesManager.POLICY_VALIDATION = new Facturae31Manager();

                DataToSign dataToSign = new();
                dataToSign.setXadesFormat(EnumFormatoFirma.XAdES_BES);
                dataToSign.setEsquema(XAdESSchemas.XAdES_132);
                dataToSign.setPolicyKey("facturae31");
                dataToSign.setAddPolicy(false);
                dataToSign.setXMLEncoding("UTF-8");
                dataToSign.setEnveloped(true);
                dataToSign.addObject(new es.mityc.javasign.xml.refs.ObjectToSign(new es.mityc.javasign.xml.refs.InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));
                dataToSign.setParentSignNode("comprobante");

                //  com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl s = new();
                dataToSign.setDocument(LoadXml(archivoXml));
                object[] objArray = (new FirmaXML()).signFile(x509Certificate, dataToSign, privateKey, provider);

                ByteArrayOutputStream fs = new();

                UtilidadTratarNodo.saveDocumentToOutputStream((Document)objArray[0], fs, true);
                firmado = fs.toByteArray();

                fs.flush();
                fs.close();

                GC.Collect();
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return firmado;
        }

        private Document LoadXml(byte[] archivoXml)
        {
            try
            {
                DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
                documentBuilderFactory.setNamespaceAware(true);
                Document document = documentBuilderFactory.newDocumentBuilder().parse(new BufferedInputStream(new ByteArrayInputStream(archivoXml)));

                return document;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        private X509Certificate LoadCertificate(string path, string password, ref PrivateKey privateKey, ref Provider provider)
        {
            try
            {
                X509Certificate certificate = null;
                privateKey = null;
                provider = null;

                var blob = blobService.GetBlob(path,"certificados");

                byte[] fileBytes = null;

                if (blob.ExistsAsync().Result)
                {
                    using (var ms = new MemoryStream())
                    {
                        blob.DownloadTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }

                // using (var fileStream = System.IO.File.OpenWrite(@"C:\Users\mbcrump\Downloads\mikepic-backup.png"))
                // {
                //     blob.DownloadTo(fileStream);
                // }

                KeyStore instance = KeyStore.getInstance("PKCS12");
                //java.io.File archivo = new java.io.File(blob.Uri.AbsoluteUri);
                // var arn = new BufferedInputStream();
                InputStream inputFileStream = new ByteArrayInputStream(fileBytes);

                instance.load(new BufferedInputStream(inputFileStream), password.ToCharArray());
                // instance.load(new BufferedInputStream(new FileInputStream(archivo)), password.ToCharArray());
                IPKStoreManager kSStore = new KSStore(instance, new PassStoreKS(password));
                java.util.List signCertificates = kSStore.getSignCertificates();

                if (signCertificates.size() > 0)
                {
                    certificate = (X509Certificate)signCertificates.get(signCertificates.size()-1);
                    privateKey = kSStore.getPrivateKey(certificate);
                    provider = kSStore.getProvider(certificate);
                    return certificate;
                }
                return certificate;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}