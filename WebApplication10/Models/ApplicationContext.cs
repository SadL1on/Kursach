using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication10.Models;

namespace Zero.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Buyer> buyers { get; set; }
        public DbSet<Сheck> checKs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
             : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

