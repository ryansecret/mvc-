using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hsr.Models
{ 
    public class CommTestPlanRepository : ICommTestPlanRepository
    {
        HsrContext context = new HsrContext();

        public IQueryable<CommTestPlan> All
        {
            get { return context.CommTestPlan; }
        }

        public IQueryable<CommTestPlan> AllIncluding(params Expression<Func<CommTestPlan, object>>[] includeProperties)
        {
            IQueryable<CommTestPlan> query = context.CommTestPlan;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CommTestPlan Find(decimal? id)
        {
            return context.CommTestPlan.Find(id);
        }

        public void InsertOrUpdate(CommTestPlan commtestplan)
        {
            if (commtestplan.PlanId == default(decimal?)) {
                // New entity
                context.CommTestPlan.Add(commtestplan);
            } else {
                // Existing entity
                context.Entry(commtestplan).State = EntityState.Modified;
            }
        }

        public void Delete(decimal? id)
        {
            var commtestplan = context.CommTestPlan.Find(id);
            context.CommTestPlan.Remove(commtestplan);
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

    public interface ICommTestPlanRepository : IDisposable
    {
        IQueryable<CommTestPlan> All { get; }
        IQueryable<CommTestPlan> AllIncluding(params Expression<Func<CommTestPlan, object>>[] includeProperties);
        CommTestPlan Find(decimal? id);
        void InsertOrUpdate(CommTestPlan commtestplan);
        void Delete(decimal? id);
        void Save();
    }
}