using System.Collections.Generic;
using System.Threading.Tasks;
using ApiNetCore.Entities;
using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace ApiNetCore.Repositories
{
    public class ProductosRepository : IProductosRepository
    {

        private const string collectionName = "productos";

        private readonly IMongoCollection<Producto> dbCollection;

        private readonly FilterDefinitionBuilder<Producto> filterDefinitionBuilder = Builders<Producto>.Filter;

        public ProductosRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<Producto>(collectionName);
        }

        public async Task CreateAsync(Producto producto)
        {
            if (producto == null)
                throw new ArgumentException(nameof(Producto));
            await dbCollection.InsertOneAsync(producto);
        }

        public async Task<Producto> GetProductoAsync(Guid Id)
        {
            FilterDefinition<Producto> filter = filterDefinitionBuilder.Eq(x => x.Id, Id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Producto> GetProductoPorCodigoAsync(string Codigo)
        {
            FilterDefinition<Producto> filter = filterDefinitionBuilder.Regex(
                x => x.CodigoPrincipal,
                new BsonRegularExpression(string.Format("^{0}$", Codigo), "i")
            );
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Producto> GetProductoPorDescripcionAsync(string Descripcion)
        {
            FilterDefinition<Producto> filter = filterDefinitionBuilder.Regex(
                x => x.Descripcion,
                new BsonRegularExpression(string.Format("^{0}$", Descripcion), "i")
            );
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosAsync(string IdUsuario)
        {
            FilterDefinition<Producto> filter = filterDefinitionBuilder.Eq(producto => producto.Usuario, IdUsuario);
            return await dbCollection.Find(filter).ToListAsync();
        }
    }
}