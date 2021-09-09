using System.Threading.Tasks;
using ApiNetCore.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ApiNetCore.Repositories
{
    public class EmpresasRepository : IEmpresasRepository
    {
        private const string collectionName = "empresas";

        private readonly IMongoCollection<Empresa> dbCollection;

        private readonly FilterDefinitionBuilder<Empresa> filterDefinitionBuilder = Builders<Empresa>.Filter;

        public EmpresasRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<Empresa>(collectionName);
        }
        public async Task CreateAsync(Empresa empresa)
        {
            if (empresa == null)
                throw new ArgumentException(nameof(Empresa));
            await dbCollection.InsertOneAsync(empresa);
        }

        public async Task<IReadOnlyCollection<Empresa>> GetAllAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }

        public async Task<Empresa> GetAsync(Guid Id)
        {
            FilterDefinition<Empresa> filter = filterDefinitionBuilder.Eq(empresa => empresa.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Empresa> ObtenerEmpresaPorIdentificacion(string identificacion)
        {
            FilterDefinition<Empresa> filter = filterDefinitionBuilder.Eq(empresa => empresa.Ruc, identificacion);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}