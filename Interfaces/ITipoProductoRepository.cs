using ApiNetCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiNetCore.Repositories
{
    public interface ITipoProductoRepository
    {
        Task CreateAsync(TipoProducto tipoProducto);

        Task<IEnumerable<TipoProducto>> GetAsync();
    }
}