using BuchhaltungPrivat.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuchhaltungPrivat
{
    public class HBContext : DbContext
    {
        public HBContext()
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Booking> Booking { get; set; } = null!;

        public virtual DbSet<Category> Category { get; set; } = null!;

        public virtual DbSet<Konto> Konto { get; set; } = null!;

        public virtual DbSet<StandingOrder> StandingOrder { get; set; } = null!;

        public virtual DbSet<SubCategory> SubCategory { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BuchhaltungPrivatIII;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, CategoryName = "Einnahme" });
            //modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 2, CategoryName = "Ausgabe" });

            //modelBuilder.Entity<Konto>().HasData(new Konto { KontoId = 1, KontoName = "Bank" });
            //modelBuilder.Entity<Konto>().HasData(new Konto { KontoId = 2, KontoName = "Bar" });

            //modelBuilder.Entity<SubCategory>().HasData(new SubCategory { CategoryId = 1, SubCategoryId = 1, SubCategoryName = "Startkapital" });
            //modelBuilder.Entity<SubCategory>().HasData(new SubCategory { CategoryId = 2, SubCategoryId = 1, SubCategoryName = "Startkapital" });
        }
    }

}
