using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriFitApp.Shared.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NutriFitApp.API.Data
{
    public class NutriFitDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public NutriFitDbContext(DbContextOptions<NutriFitDbContext> options) : base(options) { }

        public DbSet<Dieta> Dietas { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Dieta>()
                .HasOne(d => d.Usuario)
                .WithMany(u => u.Dietas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Dieta>()
                .HasOne(d => d.Nutriologo)
                .WithMany()
                .HasForeignKey(d => d.NutriologoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rutina>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Rutinas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rutina>()
                .HasOne(r => r.Entrenador)
                .WithMany()
                .HasForeignKey(r => r.EntrenadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
