using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_backgroundservice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Store>().HasData(new Store[] {
                new Store(1)
            });

            builder.Entity<StoreItem>().HasData(new StoreItem[] {
                new StoreItem(1, "Book", 10, 50.0f),
                new StoreItem(2,"Pan", 20, 340.5f)
            });

        }
    }
}
