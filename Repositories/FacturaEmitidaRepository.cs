using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;
using System;
namespace ApiNetCore.Repositories
{
    public class FacturaEmitidaRepository : IFacturaEmitidaRepository
    {
        private const string collectionName = "facturasemitidas";

        private readonly IMongoCollection<FacturaEmitida> dbCollection;

        private readonly FilterDefinitionBuilder<FacturaEmitida> filterDefinitionBuilder = Builders<FacturaEmitida>.Filter;

        public FacturaEmitidaRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<FacturaEmitida>(collectionName);
        }

        public async Task CreateAsync(FacturaEmitida facturaEmitida)
        {
            if (facturaEmitida == null)
                throw new ArgumentException(nameof(FacturaEmitida));
            await dbCollection.InsertOneAsync(facturaEmitida);
        }
    }
}