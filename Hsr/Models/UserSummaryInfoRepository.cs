using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hsr.Models
{ 
    public class UserSummaryInfoRepository : IUserSummaryInfoRepository
    {
        HsrContext context = new HsrContext();

        public IQueryable<UserSummaryInfo> All
        {
            get { return context.UserSummaryInfo; }
        }

        public IQueryable<UserSummaryInfo> AllIncluding(params Expression<Func<UserSummaryInfo, object>>[] includeProperties)
        {
            IQueryable<UserSummaryInfo> query = context.UserSummaryInfo;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public UserSummaryInfo Find(string id)
        {
            return context.UserSummaryInfo.Find(id);
        }

        public void InsertOrUpdate(UserSummaryInfo usersummaryinfo)
        {
            if (usersummaryinfo.Auid == default(string)) {
                // New entity
                context.UserSummaryInfo.Add(usersummaryinfo);
            } else {
                // Existing entity
                context.Entry(usersummaryinfo).State = EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var usersummaryinfo = context.UserSummaryInfo.Find(id);
            context.UserSummaryInfo.Remove(usersummaryinfo);
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

    public interface IUserSummaryInfoRepository : IDisposable
    {
        IQueryable<UserSummaryInfo> All { get; }
        IQueryable<UserSummaryInfo> AllIncluding(params Expression<Func<UserSummaryInfo, object>>[] includeProperties);
        UserSummaryInfo Find(string id);
        void InsertOrUpdate(UserSummaryInfo usersummaryinfo);
        void Delete(string id);
        void Save();
    }
}