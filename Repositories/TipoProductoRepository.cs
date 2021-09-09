using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ApiNetCore.Repositories
{
    public class TipoProductoRepository : ITipoProductoRepository
    {
        private const string collectionName = "tiposproducto";

        private readonly IMongoCollection<TipoProducto> dbCollection;

        private readonly FilterDefinitionBuilder<TipoProducto> filterDefinitionBuilder = Builders<TipoProducto>.Filter;

        public TipoProductoRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<TipoProducto>(collectionName);
        }
        public async Task CreateAsync(TipoProducto tipoProducto)
        {
            if (tipoProducto == null)
                throw new ArgumentException(nameof(TipoProducto));
            await dbCollection.InsertOneAsync(tipoProducto);
        }

        public async Task<IEnumerable<TipoProducto>> GetAsync()
        {
            FilterDefinition<TipoProducto> filter = filterDefinitionBuilder.Eq(tipo => tipo.Activo,true);
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}