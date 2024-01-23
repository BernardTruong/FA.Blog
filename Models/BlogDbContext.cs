using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FA.JustBlog.Core.Models
{
    public class BlogDbContext : IdentityDbContext
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<PostTag>().HasKey(sc => new { sc.PostId, sc.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(sc => sc.Post)
                .WithMany(s => s.PostTag)
                .HasForeignKey(sc => sc.PostId);


            modelBuilder.Entity<PostTag>()
                .HasOne(sc => sc.Tag)
                .WithMany(s => s.PostTag)
                .HasForeignKey(sc => sc.TagId);

            modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "For Fun",
                UrlSlug = "abc.com",
                Description = "No"
            },
            new Category
            {
                Id = 2,
                Name = "For Cheer",
                UrlSlug = "andi.com",
                Description = "No"
            },
            new Category
            {
                Id = 3,
                Name = "For Work",
                UrlSlug = "soccer.com",
                Description = "No"
            }
            );


            modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "SuperMan",
                Description = "Something",
                Content = "St",
                UrlSlug = "abc",
                PostedOn = new DateTime(2023,01,01),
                Modified = false,
                CategoryId = 1,
                Published = true,
                ViewCount = 1,
                RateCount = 1,  
                TotalRate = 2
            },
            new Post
            {
                Id = 2,
                Title = "Ninja",
                Description = "Something",
                Content = "St",
                UrlSlug = "abc",
                PostedOn = new DateTime(2023, 01, 01),
                Modified = false,
                CategoryId = 2,
                Published = false,
                ViewCount = 1,
                RateCount = 1,
                TotalRate = 3
            },
            new Post
            {
                Id = 3,
                Title = "WorkLifeBalance",
                Description = "Something",
                Content = "St",
                UrlSlug = "abc",
                PostedOn = new DateTime(2023, 01, 01),
                Modified = false,
                CategoryId = 3,
                Published = false,
                ViewCount = 1,
                RateCount = 1,
                TotalRate = 4
            }
            );


            modelBuilder.Entity<Tag>().HasData(
            new Tag
            {
                TagId = 1,
                Name = "SuperMan2",
                Description = "Something",
                UrlSlug = "abc",
                Count = 1,
            },
            new Tag
            {
                TagId = 2,
                Name = "Avatar2",
                Description = "Something",
                UrlSlug = "abc",
                Count = 2,
            },
            new Tag
            {
                TagId = 3,
                Name = "Sigma2",
                Description = "Something",
                UrlSlug = "abc",
                Count = 3,
            }
            );


            modelBuilder.Entity<PostTag>().HasData(
            new PostTag { PostId = 1, TagId = 1 },
            new PostTag { PostId = 2, TagId = 2 },
            new PostTag { PostId = 3, TagId = 3 }
            );
        }
    }
}
