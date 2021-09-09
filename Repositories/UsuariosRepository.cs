using System;
using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;

namespace ApiNetCore.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private const string collectionName = "usuarios";

        private readonly IMongoCollection<Usuario> dbCollection;

        private readonly FilterDefinitionBuilder<Usuario> filterDefinitionBuilder = Builders<Usuario>.Filter;

        public UsuariosRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<Usuario>(collectionName);
        }
        public async Task CreateAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException(nameof(usuario));
            await dbCollection.InsertOneAsync(usuario);
        }

        public async Task<Usuario> ObtenerPorIdAsync(Guid Id)
        {
            FilterDefinition<Usuario> filter = filterDefinitionBuilder.Eq(usuario => usuario.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObtenerUsuarioPorLoginAsync(string login)
        {
            FilterDefinition<Usuario> filter = filterDefinitionBuilder.Eq(usuario => usuario.Email, login);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}