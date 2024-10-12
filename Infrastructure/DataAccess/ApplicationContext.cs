using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }


        // DbSets for your models
        public DbSet<Articles> Articles { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ArticlesPermission> ArticlesPermissions { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<ArticleKeyword> ArticleKeywords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ArticlesPermission
            modelBuilder.Entity<ArticlesPermission>()
                .HasKey(ap => ap.Id);

            modelBuilder.Entity<ArticlesPermission>()
                .HasOne(ap => ap.Article)
                .WithMany(a => a.ArticlesPermissions)
                .HasForeignKey(ap => ap.ArticlesId);

            modelBuilder.Entity<ArticlesPermission>()
                .HasOne(ap => ap.Category)
                .WithMany(c => c.ArticlesPermissions)
                .HasForeignKey(ap => ap.CategoriesId);

            //ArticleKeyword 
            modelBuilder.Entity<ArticleKeyword>()
                .HasKey(ak => new { ak.ArticleId, ak.KeywordId });

            modelBuilder.Entity<ArticleKeyword>()
                .HasOne(ak => ak.Article)
                .WithMany(a => a.ArticleKeywords)
                .HasForeignKey(ak => ak.ArticleId);

            modelBuilder.Entity<ArticleKeyword>()
                .HasOne(ak => ak.Keyword)
                .WithMany(k => k.ArticleKeywords)
                .HasForeignKey(ak => ak.KeywordId);

            //SubTitle
           modelBuilder.Entity<Articles>()
            .Property<string>("SubTitle")
            .HasComputedColumnSql("CONVERT(nvarchar(max), [Title]) + ' - ' + CONVERT(nvarchar(max), [RegDate], 120)");

        }
    }
}

