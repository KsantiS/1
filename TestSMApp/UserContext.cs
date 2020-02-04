using System;
using System.Collections.Generic;
using System.Data.Entity;
using TestSMApp.Model;
 
namespace TestSMApp
{
    class UserContext : DbContext
    {
        public UserContext()
            : base("DbConnection")
        { }

        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<RemarkName> RemarkNames { get; set; }
        public DbSet<InspectionName> InspectionNames { get; set; }
        public DbSet<Inspector> Inspectors { get; set; }

    }
}