using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ApiNetCore.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public BlobClient GetBlob(string nombre, string contenedor)
        {
            var containerClient = _blobClient.GetBlobContainerClient(contenedor);

            var blobClient = containerClient.GetBlobClient(nombre);

            return blobClient;
        }

        public async Task UploadBlob(string nombre, string contenedor, byte[] contenido)
        {
            var containerClient = _blobClient.GetBlobContainerClient(contenedor);

            var blobClient = containerClient.GetBlobClient(nombre);

            using(var stream = new MemoryStream(contenido, writable: false)) {
                await blobClient.UploadAsync(stream);
            }
            
        }
    }
}