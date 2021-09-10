using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ApiNetCore.Repositories
{
    public class ClientesRepository : IClientesRepository
    {
        private const string collectionName = "clientes";

        private readonly IMongoCollection<Cliente> dbCollecion;

        private readonly FilterDefinitionBuilder<Cliente> filterDefinitionBuilder = Builders<Cliente>.Filter;

        public ClientesRepository(IMongoDatabase database)
        {
            dbCollecion = database.GetCollection<Cliente>(collectionName);
        }
        public async Task CreateAsync(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentException(nameof(Cliente));
            await dbCollecion.InsertOneAsync(cliente);
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync(string IdUsuario)
        {
            FilterDefinition<Cliente> filter = filterDefinitionBuilder.Eq(x => x.Usuario, IdUsuario);
            return await dbCollecion.Find(filter).ToListAsync();
        }
    }
}