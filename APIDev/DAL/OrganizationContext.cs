using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDev.Models;
using Microsoft.EntityFrameworkCore;


namespace APIDev.DAL
{
    public class OrganizationContext : DbContext
    {
        public OrganizationContext(DbContextOptions<OrganizationContext> options) : base(options)
        {

        }

        public DbSet<NPO> NPOs { get; set; }
        public DbSet<SiteNames> SiteNamess { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NPO>().ToTable("NPO");
        }
    }
}
