using DataAccess.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class ZeniAppDbContext : DbContext
    {
        public ZeniAppDbContext(DbContextOptions<ZeniAppDbContext> options) : base(options)
        {

        }

        public ZeniAppDbContext()
        {
            Database.EnsureCreated();
            try
            {
                Database.Migrate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<ZeniAppSettings> Settings { get; set; }
    }
}
