using Domain.Entities;
using Domain.Entities.KompTech;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : IdentityDbContext<ApplicationUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<NoteBook> NoteBooks { get; set; }
    public DbSet<SmartPhone> SmartPhones { get; set; }
    public DbSet<Tablet> Tablets { get; set; }
    public DbSet<ProfileUser> Profiles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
    //   public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Picture> Pictures { get; set; }

    public DbSet<Roles> Roles { get; set; }
}
