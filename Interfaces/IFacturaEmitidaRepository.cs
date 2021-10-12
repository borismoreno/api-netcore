using ApiNetCore.Entities;
using System.Threading.Tasks;
namespace ApiNetCore.Repositories
{
    public interface IFacturaEmitidaRepository
    {
        Task CreateAsync(FacturaEmitida facturaEmitida);

        Task<FacturaEmitida> GetPorClaveAcceso(string claveAcceso);
    }
}