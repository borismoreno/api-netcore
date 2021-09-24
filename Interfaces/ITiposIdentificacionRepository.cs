using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;
namespace ApiNetCore.Repositories
{
    public interface ITiposIdentificacionRepository
    {
        Task CreateAsync(TipoIdentificacion tipoIdentificacion);

        Task<IEnumerable<TipoIdentificacion>> GetTiposIdentificacionAsync();

        Task<TipoIdentificacion> GetAsync(Guid id);
    }
}