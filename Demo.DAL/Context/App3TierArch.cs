using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Context
{
    public class App3TierArch : IdentityDbContext<ApplicationUser>
    {
        
        public App3TierArch(DbContextOptions<App3TierArch> options ) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    =>optionsBuilder.UseSqlServer("Server = .; Database = App3TierArc; Trusted_Connection = true; MultipleActiveResultSets = true");
        

        public DbSet<Department> Departments { get; set; }   
        public DbSet<Employee> Employees { get; set; }

         
    }
}
