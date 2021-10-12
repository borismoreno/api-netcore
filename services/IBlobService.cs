using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ApiNetCore.Services
{
    public interface IBlobService
    {
        BlobClient GetBlob(string nombre, string contenedor);

        Task UploadBlob(string nombre, string contenedor, byte[] contenido);
    }
}