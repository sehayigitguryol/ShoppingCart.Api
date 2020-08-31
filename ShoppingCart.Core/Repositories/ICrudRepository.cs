using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Repositories
{
    /// <summary>
    /// Basic CRUD repository for all other repositories
    /// </summary>
    /// <typeparam name="TEntity">Entity that is stored on DB</typeparam>
    /// <typeparam name="TKey"> key type of entity</typeparam>
    public interface ICrudRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);

        void Delete(TKey id);

        TEntity Find(TKey id);

        IEnumerable<TEntity> GetAll();

        void Update(TEntity entity);
    }
}
