using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Hsr.Model.Models;

namespace Hsr.Models
{ 
    public class SysroleRepository : ISysroleRepository
    {
        HsrContext context = new HsrContext();

        public IQueryable<Sysrole> All
        {
            get { return context.Sysrole; }
        }

        public IQueryable<Sysrole> AllIncluding(params Expression<Func<Sysrole, object>>[] includeProperties)
        {
            IQueryable<Sysrole> query = context.Sysrole;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Sysrole Find(decimal? id)
        {
            return context.Sysrole.Find(id);
        }

        public void InsertOrUpdate(Sysrole sysrole)
        {
            if (sysrole.RoleId == default(decimal?)) {
                // New entity
                context.Sysrole.Add(sysrole);
            } else {
                // Existing entity
                context.Entry(sysrole).State = EntityState.Modified;
            }
        }

        public void Delete(decimal? id)
        {
            var sysrole = context.Sysrole.Find(id);
            context.Sysrole.Remove(sysrole);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface ISysroleRepository : IDisposable
    {
        IQueryable<Sysrole> All { get; }
        IQueryable<Sysrole> AllIncluding(params Expression<Func<Sysrole, object>>[] includeProperties);
        Sysrole Find(decimal? id);
        void InsertOrUpdate(Sysrole sysrole);
        void Delete(decimal? id);
        void Save();
    }
}