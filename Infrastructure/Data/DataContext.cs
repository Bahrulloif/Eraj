using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Entities.RealEstate;
using Domain.Entities.Transport;
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

    // Shadows IdentityDbContext<ApplicationUser>.Roles (DbSet<IdentityRole>) on purpose: the app
    // uses the derived Roles : IdentityRole everywhere (e.g. RoleManager<Roles>), and DbSet<T> is
    // invariant so `override` isn't an option here even though the base property is virtual.
    public new DbSet<Roles> Roles { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Motorbike> Motorbikes { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<SpareAccessorTransp> SpareAccessorTransps { get; set; }
    public DbSet<SpareAccessorKomp> SpareAccessorKomps { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<CommercialRealEstate> CommercialRealEstates { get; set; }
    public DbSet<Cottage> Cottages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Picture>().HasIndex(p => new { p.ProductType, p.ProductId });
    }
}
