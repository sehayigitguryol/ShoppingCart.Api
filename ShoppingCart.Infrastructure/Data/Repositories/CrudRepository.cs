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
            await Task.Run(() => _dbCollection.InsertOneAsync(entity));
        }

        public async Task<bool> Delete(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);

            DeleteResult deleteResult = await _dbCollection.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
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

        public async Task<bool> Update(TEntity entity)
        {
            ReplaceOneResult updateResult = await _dbCollection.ReplaceOneAsync(
                filter: g => g.Id == entity.Id,
                replacement: entity);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
