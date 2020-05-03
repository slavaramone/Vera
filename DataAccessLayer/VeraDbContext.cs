using DataAccessLayer.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer
{
    public class VeraDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserSmsSession> UserSmsSessions { get; set; }

        public VeraDbContext(DbContextOptions<VeraDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSmsSession>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserSessions)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Measurement>()
                .HasOne(x => x.User)
                .WithMany(x => x.Measurements)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Users
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"), LastName = "admin", FirstName = "admin", Patronymic = "admin", BirthDate = new DateTime(2000, 1, 1), Role = UserRole.Admin, Phone = "79876649344"
            });
		}
    }
}
