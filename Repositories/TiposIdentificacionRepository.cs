using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;
using System;
using MongoDB.Driver;

namespace ApiNetCore.Repositories
{
    public class TiposIdentificacionRepository : ITiposIdentificacionRepository
    {
        private const string collectionName = "tiposidentificacion";

        private readonly IMongoCollection<TipoIdentificacion> dbCollection;

        private readonly FilterDefinitionBuilder<TipoIdentificacion> filterDefinitionBuilder = Builders<TipoIdentificacion>.Filter;

        public TiposIdentificacionRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<TipoIdentificacion>(collectionName);
        }
        public async Task CreateAsync(TipoIdentificacion tipoIdentificacion)
        {
            if (tipoIdentificacion == null)
                throw new ArgumentException(nameof(TipoIdentificacion));
            await dbCollection.InsertOneAsync(tipoIdentificacion);
        }

        public async Task<TipoIdentificacion> GetAsync(Guid id)
        {
            FilterDefinition<TipoIdentificacion> filter = filterDefinitionBuilder.Eq(x => x.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TipoIdentificacion>> GetTiposIdentificacionAsync()
        {
            FilterDefinition<TipoIdentificacion> filter = filterDefinitionBuilder.Eq(x => x.Activo, true);
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}