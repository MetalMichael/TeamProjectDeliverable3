using System.Data.Entity;

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
    }
}