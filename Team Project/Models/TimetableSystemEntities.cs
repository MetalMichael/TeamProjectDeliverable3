using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TimetableSystem.Models
{
    public class TimetableSystemEntities : DbContext
    {
        public TimetableSystemEntities() : base("DefaultConnection") { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RequestWeek> RequestWeeks { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Entity<Request>().HasMany(r => r.RequestWeeks).WithRequired(a => a.Request).HasForeignKey(r => r.RequestId);
        }
    }
}