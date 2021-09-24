using ApiNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiNetCore.Repositories
{
    public interface ITarifasIvaRepository
    {
        Task CreateAsync(TarifaIva tarifaIva);

        Task<IEnumerable<TarifaIva>> GetAsync();

        Task<TarifaIva> GetTarifaIvaAsync(Guid Id);
    }
}