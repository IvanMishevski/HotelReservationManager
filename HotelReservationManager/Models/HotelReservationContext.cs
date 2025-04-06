using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotelReservationManager.Models
{
    public class HotelReservationContext : IdentityDbContext
    {
        public HotelReservationContext(DbContextOptions<HotelReservationContext> options)
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        // DbSet for each table
        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationClient> ReservationClients { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            // Many-to-many relationship between reservations and clients
            // Make UserId nullable in the Reservation entity
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Ensuring UserId is nullable
            modelBuilder.Entity<Reservation>()
                .Property(r => r.UserId)
                .IsRequired(false); // This will ensure that UserId is nullable

            // Many-to-many relationship between Reservations and Clients
            modelBuilder.Entity<ReservationClient>()
                .HasKey(rc => new { rc.ReservationId, rc.ClientId });

            modelBuilder.Entity<ReservationClient>()
                .HasOne(rc => rc.Reservation)
                .WithMany(r => r.ReservationClients)
                .HasForeignKey(rc => rc.ReservationId);

            modelBuilder.Entity<ReservationClient>()
                .HasOne(rc => rc.Client)
                .WithMany(c => c.ReservationClients)
                .HasForeignKey(rc => rc.ClientId);

            // Configure Room prices as decimals with specific precision and scale
            modelBuilder.Entity<Room>()
                .Property(r => r.PriceForAdult)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .Property(r => r.PriceForChild)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.AmountDue)
                .HasColumnType("decimal(18,2)");

            // Seed Admin Role
            var adminRoleId = "1";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString() // Replace with static GUID
                }
            );

            // Seed Admin User
            var adminUserId = "1";
            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@hotel.com",
                    NormalizedEmail = "ADMIN@HOTEL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0888888888",
                    PhoneNumberConfirmed = true,

                    // User-specific required properties
                    FirstName = "Admin",
                    MiddleName = "System",
                    LastName = "Administrator",
                    EGN = "0000000000", // Optional, can be null
                    HireDate = DateTime.Now.Date,
                    IsActive = true,
                    TerminationDate = null // Optional
                }
            );

            // Assign Admin Role to Admin User
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );
            // Seed Data

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = 101, Capacity = 2, RoomType = "Double Room", IsAvailable = true, PriceForAdult = 50, PriceForChild = 30, ImageUrl = "https://media.hotel7dublin.com/image/upload/f_auto,g_auto,c_auto,w_3840,q_auto/v1708595213/Uploads/Hotel7/Cosy_Room_Hero_643fdf08b9.jpg" },
                new Room { Id = 2, RoomNumber = 102, Capacity = 4, RoomType = "Apartment", IsAvailable = true, PriceForAdult = 120, PriceForChild = 60, ImageUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2c/b0/b6/2b/apartment-hotels.jpg?w=1200&h=-1&s=1" },
                new Room { Id = 3, RoomNumber = 103, Capacity = 2, RoomType = "Room with Double Bed", IsAvailable = true, PriceForAdult = 75, PriceForChild = 40, ImageUrl = "https://image-tc.galaxy.tf/wijpeg-bxdv8c6ji6oftcroue33x7hfl/double-duble_standard.jpg?crop=122%2C0%2C1757%2C1318" },
                new Room { Id = 4, RoomNumber = 104, Capacity = 1, RoomType = "Penthouse", IsAvailable = true, PriceForAdult = 200, PriceForChild = 100, ImageUrl = "https://www.paranych.com/uploads/benefits-penthouse-living-main-image.png" }
            );


            modelBuilder.Entity<Client>().HasData(
                new Client { Id = "1", FirstName = "Alexander", LastName = "Atanasov", PhoneNumber = "0876543210", Email = "alex.atanasov@example.com", IsAdult = true },
                new Client { Id = "2", FirstName = "Stefka", LastName = "Dimitrova", PhoneNumber = "0896543210", Email = "stefka.dimitrova@example.com", IsAdult = true }
            );    
        }
    }
}
