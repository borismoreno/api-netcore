using System.Threading.Tasks;
using ApiNetCore.Entities;
using System.Collections.Generic;
using System;

namespace ApiNetCore.Repositories
{
    public interface IProductosRepository
    {
        Task CreateAsync(Producto producto);

        Task<IEnumerable<Producto>> GetProductosAsync(string IdUsuario);

        Task<Producto> GetProductoPorCodigoAsync(string Codigo);

        Task<Producto> GetProductoAsync(Guid Id);
        
        Task<Producto> GetProductoPorDescripcionAsync(string Descripcion);
    }
}