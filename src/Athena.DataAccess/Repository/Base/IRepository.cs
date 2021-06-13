using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Athena.DataAccess.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="obj"></param>
        void Delete(T obj);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="obj">Entity to update</param>
        void Update(T obj);
        /// <summary>
        /// Get all objects, returns query, does not execute it
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllQuery();
        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Get all objects asynchronously
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();
        /// <summary>
        /// Find objects that match the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> Find(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Find objects that match the specified criteria, returns query, does not execute it
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> FindQuery(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Get object by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(long id);
        /// <summary>
        /// Get object by its ID asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ValueTask<T> GetByIdAsync(long id);

        /// <summary>
        /// Commits all changes made to the Database
        /// </summary>
        /// <returns></returns>
        int CommitTransaction();

        /// <summary>
        /// Commits all changes made to the Database asynchronously
        /// </summary>
        /// <returns></returns>
        Task<int> CommitTransactionAsync();
    }
}