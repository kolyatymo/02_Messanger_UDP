using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _03_task_db
{
    public class dbNumberCars : DbContext
    {
        public dbNumberCars()
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"
                                Data Source = (localdb)\MSSQLLocalDB;
                                Initial Catalog = db_Region_Code;
                                Integrated Security = True;
                                Connect Timeout = 2;                       
                                Encrypt = False;
                                Trust Server Certificate = False;
                                Application Intent = ReadWrite;
                                Multi Subnet Failover = False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>()
                .Property(x => x.RegionCode)
                .HasMaxLength(5);

            modelBuilder.Entity<Region>()
                .Property(x => x.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<Car>()
                .HasOne(o => o.Region)
                .WithMany(m => m.Cars)
                .HasForeignKey(f => f.RegionId);
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Region> Region { get; set; }
    }
    public class Car
    {
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }

    }

    public class Region
    {
        public Region()
        {
            Cars = new HashSet<Car>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
