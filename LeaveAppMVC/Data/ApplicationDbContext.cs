using Microsoft.EntityFrameworkCore;
using LeaveAppMVC.Models;

namespace LeaveAppMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        { }

        public DbSet<LeaveApplicationModel> LeaveApplications { get; set; }
    }
}
