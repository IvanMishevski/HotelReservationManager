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

            modelBuilder.Entity<Room>()
                .Property(r => r.PriceForAdult)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .Property(r => r.PriceForChild)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.AmountDue)
                .HasColumnType("decimal(18,2)");

            // UserRole primary key
  

            // Seed Data

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = 101, Capacity = 2, RoomType = "Double Room", IsAvailable = true, PriceForAdult = 50, PriceForChild = 30, ImageUrl = "https://media.hotel7dublin.com/image/upload/f_auto,g_auto,c_auto,w_3840,q_auto/v1708595213/Uploads/Hotel7/Cosy_Room_Hero_643fdf08b9.jpg" },
                new Room { Id = 2, RoomNumber = 102, Capacity = 4, RoomType = "Apartment", IsAvailable = true, PriceForAdult = 120, PriceForChild = 60, ImageUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2c/b0/b6/2b/apartment-hotels.jpg?w=1200&h=-1&s=1" },
                new Room { Id = 3, RoomNumber = 103, Capacity = 2, RoomType = "Room with Double Bed", IsAvailable = true, PriceForAdult = 75, PriceForChild = 40, ImageUrl = "https://image-tc.galaxy.tf/wijpeg-bxdv8c6ji6oftcroue33x7hfl/double-duble_standard.jpg?crop=122%2C0%2C1757%2C1318" },
                new Room { Id = 4, RoomNumber = 104, Capacity = 1, RoomType = "Penthouse", IsAvailable = true, PriceForAdult = 200, PriceForChild = 100, ImageUrl = "https://www.paranych.com/uploads/benefits-penthouse-living-main-image.png" }
            );


            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "Alexander", LastName = "Atanasov", PhoneNumber = "0876543210", Email = "alex.atanasov@example.com", IsAdult = true },
                new Client { Id = 2, FirstName = "Stefka", LastName = "Dimitrova", PhoneNumber = "0896543210", Email = "stefka.dimitrova@example.com", IsAdult = true }
            );    
        }
    }
}
