using System.Collections.Generic;
using System.Linq;

namespace Hsr.Data.Interface
{
    public partial interface IRepository<T> where T : BaseModel
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Insert(T entity);

        bool BulkInsert(List<T> entities);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Update(T entity);


        bool UpdateWithNav(T entity);
        
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Delete(T entity);

        bool Delete(object key);
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        T Copy(T needCopy);

        IEnumerable<T> SqlQuery(string sql, params object[] parameters);

       
        int ExecuteSqlCommand(string sql, params object[] parameters);

        void Dispose();
    }
}
