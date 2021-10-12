using System.Threading.Tasks;
using ApiNetCore.Middlewares;
using ApiNetCore.Repositories;
using ApiNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WkHtmlToPdfDotNet.Contracts;

namespace ApiNetCore.Controllers
{
    [ApiController]
    [Route("api/comprobante")]
    [Authorize]
    public class ComprobanteController : ControllerBase
    {
        private readonly IBlobService blobService;
        private readonly IConverter converter;
        private readonly IFacturaEmitidaRepository facturaEmitidaRepository;
        private readonly IClientesRepository clientesRepository;

        public ComprobanteController(IBlobService blobService, 
                                        IConverter converter,
                                        IFacturaEmitidaRepository facturaEmitidaRepository,
                                        IClientesRepository clientesRepository
                                    )
        {
            this.blobService = blobService;
            this.converter = converter;
            this.facturaEmitidaRepository = facturaEmitidaRepository;
            this.clientesRepository = clientesRepository;
        }

        [HttpGet("obtener-pdf/{claveAcceso}")]
        public async Task<IActionResult> GetComprobante(string claveAcceso)
        {
            TrabajoPdf objPdf = new(blobService, facturaEmitidaRepository, clientesRepository, converter);
            byte[] document = await objPdf.CreateDocument(claveAcceso);
            // byte[] content = converter.Convert(document);
            return File(document, "application/pdf", $"{claveAcceso}.pdf");
        }
    }
}