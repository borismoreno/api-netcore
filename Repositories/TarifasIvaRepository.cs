using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;
using System;
using MongoDB.Driver;

namespace ApiNetCore.Repositories
{
    public class TarifasIvaRepository : ITarifasIvaRepository
    {
        private const string collectionName = "tarifasiva";
        private readonly IMongoCollection<TarifaIva> dbCollection;

        private readonly FilterDefinitionBuilder<TarifaIva> filterDefinitionBuilder = Builders<TarifaIva>.Filter;

        public TarifasIvaRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<TarifaIva>(collectionName);
        }
        public async Task CreateAsync(TarifaIva tarifaIva)
        {
            if (tarifaIva == null)
                throw new ArgumentException(nameof(TarifaIva));
            await dbCollection.InsertOneAsync(tarifaIva);
        }

        public async Task<IEnumerable<TarifaIva>> GetAsync()
        {
            FilterDefinition<TarifaIva> filter = filterDefinitionBuilder.Eq(tarifa => tarifa.Activo, true);
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}