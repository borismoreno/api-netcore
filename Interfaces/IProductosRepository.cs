using System.Threading.Tasks;
using ApiNetCore.Entities;
using System.Collections.Generic;
namespace ApiNetCore.Repositories
{
    public interface IProductosRepository
    {
        Task CreateAsync(Producto producto);

        Task<IEnumerable<Producto>> GetProductosAsync(string IdUsuario);
    }
}