using Microsoft.EntityFrameworkCore;
using AsyncInn.Models;
using AsyncInn.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace AsyncInn.Data
{
    public class AsyncInnDbContext : DbContext
    {
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RoomAmenities> RoomAmenities { get; set; }
    public DbSet<HotelRoom> HotelRoom { get; set; }

    public AsyncInnDbContext(DbContextOptions options) : base(options)
    {
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      Amenity AirConditioning = new Amenity { Id = 51, Name = "Air Conditioning" };
      Amenity OceanView = new Amenity { Id = 52, Name = "Ocean View" };
      Amenity MiniBar = new Amenity { Id = 53, Name = "Mini Bar" };
      Amenity Safe = new Amenity { Id = 54, Name = "Safe" };
      Amenity Heating = new Amenity { Id = 55, Name = "Heating" };


      Hotel TheNemo = new Hotel { Id = 100, StreetAddress = "P.Sherman 42", City = "Wallaby Way", State = "Sydney", Name = "The Nemo", Phone = "555-555-5555", Country = "Australia" };
      Hotel TheGreenDay = new Hotel { Id = 200, StreetAddress = "321 Broken Dreams Boulevard", City = "Greenday", State = "Ohio", Name = "The Green Day", Phone = "867-5309", Country = "America" };
      Hotel TheBroadway = new Hotel { Id = 300, StreetAddress = "123 Hollywood Drive", City = "Hollywood", State = "Iowa", Name = "The Broadway", Phone = "911-911-9111", Country = "America" };


      Room TheButt = new Room { Id = 101, Layout = 2, Name = "The Butt" };
      Room TheAmericanIdiot = new Room { Id = 201, Layout = 0, Name = "The American Idiot" };
      Room TheGlobe = new Room { Id = 301, Layout = 1, Name = "The Globe" };

      HotelRoom Room001 = new HotelRoom { Id = 91, RoomID = TheButt.Id, HotelID = TheNemo.Id, PetFriendly = true, Rate = 100.73m, RoomNumber = 001 };
      HotelRoom Room002 = new HotelRoom { Id = 92, HotelID = TheGreenDay.Id, RoomID = TheAmericanIdiot.Id, PetFriendly = false, Rate = 73.99m, RoomNumber = 002 };
      HotelRoom Room003 = new HotelRoom { Id = 93, RoomID = TheGlobe.Id, HotelID = TheBroadway.Id, PetFriendly = true, Rate = 398.73m, RoomNumber = 003 };

      RoomAmenities ra1 = new RoomAmenities { AmenitiesID = AirConditioning.Id, RoomID = TheButt.Id, Id = 71 };
      RoomAmenities ra2 = new RoomAmenities { AmenitiesID = MiniBar.Id, Id = 72, RoomID = TheAmericanIdiot.Id };
      RoomAmenities ra3 = new RoomAmenities { AmenitiesID = OceanView.Id, RoomID = TheGlobe.Id, Id = 73 };
      RoomAmenities ra4 = new RoomAmenities { AmenitiesID = Safe.Id, RoomID = TheAmericanIdiot.Id, Id = 74 };
      RoomAmenities ra5 = new RoomAmenities { AmenitiesID = Heating.Id, RoomID = TheGlobe.Id, Id = 75 };

      modelBuilder.Entity<Hotel>().HasData(TheNemo);
      modelBuilder.Entity<Hotel>().HasData(TheGreenDay);
      modelBuilder.Entity<Hotel>().HasData(TheBroadway);

      modelBuilder.Entity<Amenity>().HasData(AirConditioning);
      modelBuilder.Entity<Amenity>().HasData(OceanView);
      modelBuilder.Entity<Amenity>().HasData(MiniBar);
      modelBuilder.Entity<Amenity>().HasData(Safe);
      modelBuilder.Entity<Amenity>().HasData(Heating);

      modelBuilder.Entity<Room>().HasData(TheButt);
      modelBuilder.Entity<Room>().HasData(TheAmericanIdiot);
      modelBuilder.Entity<Room>().HasData(TheGlobe);

      modelBuilder.Entity<RoomAmenities>().HasData(ra1);
      modelBuilder.Entity<RoomAmenities>().HasData(ra2);
      modelBuilder.Entity<RoomAmenities>().HasData(ra3);
      modelBuilder.Entity<RoomAmenities>().HasData(ra4);
      modelBuilder.Entity<RoomAmenities>().HasData(ra5);

      modelBuilder.Entity<HotelRoom>().HasData(Room001);
      modelBuilder.Entity<HotelRoom>().HasData(Room002);
      modelBuilder.Entity<HotelRoom>().HasData(Room003);
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
