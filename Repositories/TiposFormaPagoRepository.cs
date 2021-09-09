using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;

namespace ApiNetCore.Repositories
{
    public class TiposFormaPagoRepository : ITiposFormaPagoRepository
    {
        private const string collectionName = "tiposformapago";

        private readonly IMongoCollection<TipoFormaPago> dbCollection;

        private readonly FilterDefinitionBuilder<TipoFormaPago> filterDefinitionBuilder = Builders<TipoFormaPago>.Filter;

        public TiposFormaPagoRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<TipoFormaPago>(collectionName);
        }
        public async Task CreateAsync(TipoFormaPago tipoFormaPago)
        {
            if (tipoFormaPago == null)
                throw new ArgumentException(nameof(TipoFormaPago));
            await dbCollection.InsertOneAsync(tipoFormaPago);
        }

        public async Task<IEnumerable<TipoFormaPago>> GetAsync()
        {
            FilterDefinition<TipoFormaPago> filter = filterDefinitionBuilder.Eq(tipo => tipo.Activo, true);
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}