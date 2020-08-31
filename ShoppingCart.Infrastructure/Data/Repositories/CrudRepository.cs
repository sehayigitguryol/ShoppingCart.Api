using MongoDB.Bson;
using MongoDB.Driver;
using ShoppingCart.Core.Entities;
using ShoppingCart.Core.Repositories;
using ShoppingCart.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Data.Repositories
{
    public abstract class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IShoppingCartContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected CrudRepository(IShoppingCartContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task Add(TEntity entity)
        {
            await _dbCollection.InsertOneAsync(entity);
        }

        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            _dbCollection.FindOneAndDelete(filter);
        }

        public async Task<TEntity> Find(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var entities = await _dbCollection.FindAsync(_ => true);

            return await entities.ToListAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity);
        }
    }
}
