using Microsoft.EntityFrameworkCore;
using PracticeAPI.Domain.Entities;

namespace PracticeAPI.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Employee> Employees { get; set; } 
    }
}
