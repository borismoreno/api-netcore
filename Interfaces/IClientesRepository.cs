using System.Threading.Tasks;
using ApiNetCore.Entities;
using System;
using System.Collections.Generic;

namespace ApiNetCore.Repositories
{
    public interface IClientesRepository
    {
        Task CreateAsync(Cliente cliente);

        Task<IEnumerable<Cliente>> GetClientesAsync(string IdUsuario);

        Task<Cliente> GetAsync(Guid Id);
    }
}