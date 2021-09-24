using System.Threading.Tasks;
using Azure.Storage.Blobs;
using java.io;

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
            // var inp = new BufferedInputStream(info.Value.Content);

            return blobClient;
        }
    }
}