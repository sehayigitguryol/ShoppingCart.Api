using ShoppingCart.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Repositories
{
    /// <summary>
    /// Basic CRUD repository for all other repositories
    /// </summary>
    /// <typeparam name="TEntity">Entity that is stored on DB</typeparam>
    /// <typeparam name="TKey"> key type of entity</typeparam>
    public interface ICrudRepository<TEntity> where TEntity : BaseEntity
    {

        Task Add(TEntity entity);

        Task<bool> Delete(string id);

        Task<TEntity> Find(string id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<bool> Update(TEntity entity);
    }
}
