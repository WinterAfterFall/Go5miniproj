using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // บังคับให้เก็บ Enum เป็น String ใน Database เสมอ
            modelBuilder.Entity<User>()
                .Property(u => u.RoleType)
                .HasConversion<string>();
        }
    }
}