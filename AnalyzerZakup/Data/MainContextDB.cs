using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup.Data
{
    public class MainContextDB : DbContext
    {
        //public DbSet<ProtocolInfo> ProtocolInfos { get; set; }

        //public MainContextDB(DbContextOptions<MainContextDB> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Author>().ToTable("authors");
        //    modelBuilder.Entity<Author>().Property(x => x.Name).IsRequired();
        //    modelBuilder.Entity<Author>().Property(x => x.Name).HasMaxLength(50);
        //    modelBuilder.Entity<Author>().HasIndex(x => x.Name).IsUnique();

        //    modelBuilder.Entity<AuthorDetail>().ToTable("author_details");
        //    modelBuilder.Entity<AuthorDetail>().HasOne(x => x.Author).WithOne(x => x.Detail).HasPrincipalKey<AuthorDetail>(x => x.Id);

        //    modelBuilder.Entity<Book>().ToTable("books");
        //    modelBuilder.Entity<Book>().Property(x => x.Title).IsRequired();
        //    modelBuilder.Entity<Book>().Property(x => x.Title).HasMaxLength(250);
        //    modelBuilder.Entity<Book>().HasOne(x => x.Author).WithMany(x => x.Books).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Category>().ToTable("categories");
        //    modelBuilder.Entity<Category>().Property(x => x.Title).IsRequired();
        //    modelBuilder.Entity<Category>().Property(x => x.Title).HasMaxLength(100);
        //    modelBuilder.Entity<Category>().HasMany(x => x.Books).WithMany(x => x.Categories).UsingEntity(t => t.ToTable("books_categories"));
        //}            
    }
}
