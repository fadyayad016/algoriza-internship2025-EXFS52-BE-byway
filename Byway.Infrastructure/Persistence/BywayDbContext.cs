using Byway.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Infrastructure.Persistence
{
    public class BywayDbContext: IdentityDbContext  <AppUser, IdentityRole<int>, int>   
    {
        public  BywayDbContext(DbContextOptions<BywayDbContext> options) : base(options) { }
        
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Content> Contents { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            
            builder.Entity<Category>()
                .HasMany(c => c.Courses)
                .WithOne(e => e.Category)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Instructor>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .Property(o => o.TotalAmount)   
                .HasColumnType("decimal(18,2)");


           
            builder.Entity<Course>()
                .HasMany(c => c.Contents)
                .WithOne(co => co.Course)
                .HasForeignKey(co => co.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
