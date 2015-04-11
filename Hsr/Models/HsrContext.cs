using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hsr.Models
{
    public class HsrContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Hsr.Models.HsrContext>());

        public DbSet<Hsr.Models.UserSummaryInfo> UserSummaryInfo { get; set; }

        public DbSet<Hsr.Model.Models.Sysrole> Sysrole { get; set; }

        public DbSet<Hsr.Model.Models.OrganizeInfo> OrganizeInfo { get; set; }

        public DbSet<Hsr.Models.CommTestPlan> CommTestPlan { get; set; }
    }
}