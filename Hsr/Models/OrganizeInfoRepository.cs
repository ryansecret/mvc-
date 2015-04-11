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
    public class OrganizeInfoRepository : IOrganizeInfoRepository
    {
        HsrContext context = new HsrContext();

        public IQueryable<OrganizeInfo> All
        {
            get { return context.OrganizeInfo; }
        }

        public IQueryable<OrganizeInfo> AllIncluding(params Expression<Func<OrganizeInfo, object>>[] includeProperties)
        {
            IQueryable<OrganizeInfo> query = context.OrganizeInfo;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public OrganizeInfo Find(decimal? id)
        {
            return context.OrganizeInfo.Find(id);
        }

        public void InsertOrUpdate(OrganizeInfo organizeinfo)
        {
            if (organizeinfo.OrgId == default(decimal?)) {
                // New entity
                context.OrganizeInfo.Add(organizeinfo);
            } else {
                // Existing entity
                context.Entry(organizeinfo).State = EntityState.Modified;
            }
        }

        public void Delete(decimal? id)
        {
            var organizeinfo = context.OrganizeInfo.Find(id);
            context.OrganizeInfo.Remove(organizeinfo);
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

    public interface IOrganizeInfoRepository : IDisposable
    {
        IQueryable<OrganizeInfo> All { get; }
        IQueryable<OrganizeInfo> AllIncluding(params Expression<Func<OrganizeInfo, object>>[] includeProperties);
        OrganizeInfo Find(decimal? id);
        void InsertOrUpdate(OrganizeInfo organizeinfo);
        void Delete(decimal? id);
        void Save();
    }
}