using System;
using System.Threading.Tasks;
using ApiNetCore.Entities;

namespace ApiNetCore.Repositories
{
    public interface IUsuariosRepository
    {
        Task CreateAsync(Usuario usuario);

        Task<Usuario> ObtenerUsuarioPorLoginAsync(string login);

        Task<Usuario> ObtenerPorIdAsync(Guid Id);
    }
}