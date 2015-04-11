using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityFramework.Extensions;
using Hsr.Data.Interface;

namespace Hsr.Data
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseModel 
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
            
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
           
            return this.Entities.Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                entity.GenerateId();
                this.Entities.Add(entity);
                
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg +=
                            string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
            
        }

        public bool BulkInsert(List<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                foreach (var entity in entities)
                {
                    entity.GenerateId();
                    this.Entities.Add(entity);
                }
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg +=
                            string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (this._context.Entry(entity).State != EntityState.Modified)
                {
                    try
                    {
                        this._context.Entry(entity).State = EntityState.Modified;
                    }
                    catch (Exception)
                    {

                        var keyValue = entity.GetKey().GetValue(entity);
                        var model = this.GetById(keyValue);
                        if (model != null)
                        {

                            this._context.Entry(model).CurrentValues.SetValues(entity);
                        }
                    }
                }
                
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine +
                               string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                   validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
            
        }

     
        public virtual bool UpdateWithNav(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                 
                var keyValue =entity.GetKey().GetValue(entity);
                var old= this.GetById(keyValue);
                this.Entities.Remove(old);
                this.Entities.Add(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine +
                               string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                   validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }
 
        public virtual bool Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

               
                var keyValue = entity.GetKey().GetValue(entity);
                var old = this.GetById(keyValue);
                this.Entities.Remove(old);
              
                this._context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }

        public virtual bool Delete(object key)
        {
            try
            {
                
                var entity = GetById(key);
                this.Entities.Remove(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                
                throw;
            }
        }

        
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }


        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public IOrderedQueryable<T> TableApplyOrder(string property, string methodName = "OrderBy")
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);

                expr = Expression.Property(expr, pi);

                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { TableNoTracking, lambda });
            return (IOrderedQueryable<T>)result;
        }

        public T Copy(T needCopy)
        {
           return _context.LoadOriginalCopy(needCopy);
        }

        public IEnumerable<T> SqlQuery(string sql, params object[] parameters)
        {
            return this._context.SqlQuery<T>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.ExecuteSqlCommand(sql, parameters: parameters);
        }


        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
