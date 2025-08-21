using Microsoft.EntityFrameworkCore;
using ScheduleGenerator.Models;

namespace ScheduleGenerator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ScheduleItem> ScheduleItems { get; set; } 
    }
}
