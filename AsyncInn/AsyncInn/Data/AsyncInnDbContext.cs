using Microsoft.EntityFrameworkCore;
using AsyncInn.Models;
using AsyncInn.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace AsyncInn.Data
{
    public class AsyncInnDbContext : DbContext
    {
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Amenity> Amenities { get; set; }

    public AsyncInnDbContext(DbContextOptions options) : base(options)
    {
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      Amenity AirConditioning = new Amenity { Id = 51, Name = "Air Conditioning" };
      Amenity OceanView = new Amenity { Id = 52, Name = "Ocean View" };
      Amenity MiniBar = new Amenity { Id = 53, Name = "Mini Bar" };

      modelBuilder.Entity<Hotel>().HasData(new Hotel { Id = 100, StreetAddress = "P.Sherman 42", City = "Wallaby Way", State = "Sydney", Name = "The Nemo", Phone = "555-555-5555", Country="Australia" });
      modelBuilder.Entity<Hotel>().HasData(new Hotel { Id = 200, StreetAddress = "321 Broken Dreams Boulevard", City = "Greenday", State = "Ohio", Name = "The Green Day", Phone = "867-5309", Country="America" });
      modelBuilder.Entity<Hotel>().HasData(new Hotel { Id = 300, StreetAddress = "123 Hollywood Drive", City = "Hollywood", State = "Iowa", Name = "The Broadway", Phone = "911-911-9111", Country="America" });

      modelBuilder.Entity<Amenity>().HasData(AirConditioning);
      modelBuilder.Entity<Amenity>().HasData(OceanView);
      modelBuilder.Entity<Amenity>().HasData(MiniBar);

      modelBuilder.Entity<Room>().HasData(new Room { Id = 101, Layout = 2, Name = "The Butt" });
      modelBuilder.Entity<Room>().HasData(new Room { Id = 201, Layout = 0, Name = "The American Idiot" });
      modelBuilder.Entity<Room>().HasData(new Room { Id = 301, Layout = 1, Name = "The Globe" });

    }
      
  }
}

public class YourDbContextFactory: IDesignTimeDbContextFactory<AsyncInnDbContext>
{
  public AsyncInnDbContext CreateDbContext( string[] args )
  {
    IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();
    var connectionString = configuration.GetConnectionString("AzureConnectionString");
    var optionsBuilder = new DbContextOptionsBuilder<AsyncInnDbContext>();
    optionsBuilder.UseSqlServer(connectionString);

    return new AsyncInnDbContext(optionsBuilder.Options);
  }
}
