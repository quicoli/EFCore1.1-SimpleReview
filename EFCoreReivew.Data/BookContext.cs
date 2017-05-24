using EFCoreReview.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFCoreReview.Data
{
    public class BookContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<AuthorBook>()
                .HasKey(s => new { s.AuthorId, s.BookId });

            //modelbuilder.Entity<Book>().Property<DateTime>("LastModified");

            foreach (var entType in modelbuilder.Model.GetEntityTypes())
            {
                modelbuilder.Entity(entType.Name).Property<DateTime>("LastModified");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    "Server = (localdb)\\mssqllocaldb; Database= BookDataCore; Trusted_Connection = True;", options=> options.MaxBatchSize(30));
            //optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseInMemoryDatabase();
        }

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries()
                .Where (x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                item.Property("LastModified").CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
