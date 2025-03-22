using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotelReservationManager.Models
{
    public class HotelReservationContext : DbContext
    {
        public HotelReservationContext(DbContextOptions<HotelReservationContext> options)
            : base(options)
        { }

        // DbSet за всяка таблица
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationClient> ReservationClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // За свързване между резервации и клиенти (много към много)
            modelBuilder.Entity<ReservationClient>()
                .HasKey(rc => new { rc.ReservationId, rc.ClientId });

            modelBuilder.Entity<ReservationClient>()
                .HasOne(rc => rc.Reservation)
                .WithMany()
                .HasForeignKey(rc => rc.ReservationId);

            modelBuilder.Entity<ReservationClient>()
                .HasOne(rc => rc.Client)
                .WithMany()
                .HasForeignKey(rc => rc.ClientId);

            // Seed Data

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = 101, Capacity = 2, RoomType = "Двойна стая", IsAvailable = true, PriceForAdult = 50, PriceForChild = 30 },
                new Room { Id = 2, RoomNumber = 102, Capacity = 4, RoomType = "Апартамент", IsAvailable = true, PriceForAdult = 120, PriceForChild = 60 },
                new Room { Id = 3, RoomNumber = 103, Capacity = 2, RoomType = "Стая с двойно легло", IsAvailable = true, PriceForAdult = 75, PriceForChild = 40 },
                new Room { Id = 4, RoomNumber = 104, Capacity = 1, RoomType = "Пентхаус", IsAvailable = true, PriceForAdult = 200, PriceForChild = 100 }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin123", FirstName = "Иван", FatherName = "Петков", LastName = "Иванов", Egn = "1234567890", PhoneNumber = "0871234567", Email = "ivan.ivanov@example.com", DateOfHire = DateTime.Now, IsActive = true },
                new User { Id = 2, Username = "employee", Password = "emp1234", FirstName = "Мария", FatherName = "Иванова", LastName = "Петрова", Egn = "0987654321", PhoneNumber = "0898765432", Email = "maria.petrova@example.com", DateOfHire = DateTime.Now, IsActive = true }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "Александър", LastName = "Атанасов", PhoneNumber = "0876543210", Email = "alex.atanasov@example.com", IsAdult = true },
                new Client { Id = 2, FirstName = "Стефка", LastName = "Димитрова", PhoneNumber = "0896543210", Email = "stefka.dimitrova@example.com", IsAdult = true }
            );

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, RoomId = 1, UserId = 1, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(3), IsBreakfastIncluded = true, IsAllInclusive = false, AmountDue = 50 * 2 },
                new Reservation { Id = 2, RoomId = 2, UserId = 2, CheckInDate = DateTime.Now.AddDays(2), CheckOutDate = DateTime.Now.AddDays(4), IsBreakfastIncluded = true, IsAllInclusive = true, AmountDue = 120 * 4 }
            );

            modelBuilder.Entity<ReservationClient>().HasData(
                new ReservationClient { ReservationId = 1, ClientId = 1 },
                new ReservationClient { ReservationId = 2, ClientId = 2 }
            );
        }
    }
}
