using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WpfApp1.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model13")
        {
        }

        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<GuestsVisits> GuestsVisits { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Workers> Workers { get; set; }
        public virtual DbSet<WorkersPhoto> WorkersPhoto { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requests>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Requests>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<Requests>()
                .Property(e => e.Patrynomic)
                .IsUnicode(false);

            modelBuilder.Entity<Requests>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Requests>()
                .Property(e => e.DateOfBirth)
                .IsUnicode(false);

            modelBuilder.Entity<Requests>()
                .Property(e => e.IdSerial)
                .HasPrecision(4, 0);

            modelBuilder.Entity<Requests>()
                .Property(e => e.IdNumber)
                .HasPrecision(6, 0);

            modelBuilder.Entity<Requests>()
                .Property(e => e.WorkerCode)
                .HasPrecision(7, 0);

            modelBuilder.Entity<Roles>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Workers)
                .WithOptional(e => e.Roles)
                .HasForeignKey(e => e.Role);

            modelBuilder.Entity<Users>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.WorkerCode)
                .HasPrecision(7, 0);

            modelBuilder.Entity<Workers>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.Patrynomic)
                .IsUnicode(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.Depart)
                .IsUnicode(false);

            modelBuilder.Entity<Workers>()
                .Property(e => e.SecretWord)
                .IsUnicode(false);
        }
    }
}
