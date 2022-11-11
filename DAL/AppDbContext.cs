using Code.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<GroupOfProduct> GroupOfProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
    }
}
