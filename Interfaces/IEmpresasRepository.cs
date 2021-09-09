using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;

namespace ApiNetCore.Repositories
{
    public interface IEmpresasRepository
    {
        Task CreateAsync(Empresa empresa);

        Task<Empresa> ObtenerEmpresaPorIdentificacion(string identificacion);

        Task<IReadOnlyCollection<Empresa>> GetAllAsync();

        Task<Empresa> GetAsync(Guid Id);
    }
}