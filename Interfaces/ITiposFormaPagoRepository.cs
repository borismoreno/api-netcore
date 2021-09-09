using ApiNetCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiNetCore.Repositories
{
    public interface ITiposFormaPagoRepository
    {
        Task CreateAsync(TipoFormaPago tipoFormaPago);

        Task<IEnumerable<TipoFormaPago>> GetAsync();
    }
}