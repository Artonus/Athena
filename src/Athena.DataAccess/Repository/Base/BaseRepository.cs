using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Athena.DataAccess.Repository.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected readonly AthenaDbContext Context;

        public BaseRepository(AthenaDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.Delete"/>
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(T obj)
        {
            Context.Remove(obj);
        }
        
        /// <summary>
        /// <inheritdoc cref="IRepository{T}.Update"/>
        /// </summary>
        /// <param name="obj"></param>
        public void Update(T obj)
        {
            Context.Update(obj);
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.GetAllQuery"/>
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAllQuery()
        {
            return Context.Set<T>();
        }
        
        /// <summary>
        /// <inheritdoc cref="IRepository{T}.GetAll"/>
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return GetAllQuery().ToList();
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.GetAllAsync"/>
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<T>> GetAllAsync()
        {
            return GetAllQuery().Cast<T>().ToListAsync();
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.Find"/>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return FindQuery(predicate).ToList();
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.FindQuery"/>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> FindQuery(Expression<Func<T, bool>> predicate)
        {
            return GetAllQuery().Where(predicate);
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.GetById"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return Context.Find<T>(id);
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.GetByIdAsync"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ValueTask<T> GetByIdAsync(long id)
        {
            return Context.FindAsync<T>(id);
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.CommitTransaction"/>
        /// </summary>
        /// <returns></returns>
        public int CommitTransaction()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// <inheritdoc cref="IRepository{T}.CommitTransactionAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task<int> CommitTransactionAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}