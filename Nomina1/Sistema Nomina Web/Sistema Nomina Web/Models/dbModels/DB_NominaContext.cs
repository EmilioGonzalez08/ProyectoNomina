using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sistema_Nomina_Web.Models.dbModels
{
    public partial class DB_NominaContext : IdentityDbContext
    {
        public DB_NominaContext()
        {
        }

        public DB_NominaContext(DbContextOptions<DB_NominaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Incidencium> Incidencia { get; set; } = null!;
        public virtual DbSet<Nomina> Nominas { get; set; } = null!;
        public virtual DbSet<PeriodoNomina> PeriodoNominas { get; set; } = null!;
        public virtual DbSet<TipoJornadum> TipoJornada { get; set; } = null!;
        public virtual DbSet<TipoSalario> TipoSalarios { get; set; } = null!;
        public virtual DbSet<Trabajador> Trabajadors { get; set; } = null!;
        public virtual DbSet<Periodicidad> Periodicidades { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Incidencium>(entity =>
            {
                entity.HasKey(e => e.IncidenciaId)
                    .HasName("PK__Incidenc__E41133E6CD3D6597");

                entity.Property(e => e.Faltas).HasDefaultValueSql("((0))");
                entity.Property(e => e.HorasExtra).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.PeriodoNomina)
                    .WithMany(p => p.Incidencia)
                    .HasForeignKey(d => d.PeriodoNominaId)
                    .OnDelete(DeleteBehavior.Cascade) // Agregar Cascade
                    .HasConstraintName("FK__Incidenci__Perio__571DF1D5");

                entity.HasOne(d => d.Trabajador)
                    .WithMany(p => p.Incidencia)
                    .HasForeignKey(d => d.TrabajadorId)
                    .OnDelete(DeleteBehavior.Cascade) // Agregar Cascade
                    .HasConstraintName("FK__Incidenci__Traba__5629CD9C");
            });

            modelBuilder.Entity<Nomina>(entity =>
            {
                entity.Property(e => e.DescuentoFaltas).HasDefaultValueSql("((0))");
                entity.Property(e => e.FechaCalculo).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.ImporteHorasExtra).HasDefaultValueSql("((0))");
                entity.Property(e => e.Imss).HasDefaultValueSql("((0))");
                entity.Property(e => e.Isr).HasDefaultValueSql("((0))");
                entity.Property(e => e.OtrasDeducciones).HasDefaultValueSql("((0))");
                entity.Property(e => e.SalarioNeto).HasDefaultValueSql("((0))");
                entity.Property(e => e.TotalDeducciones).HasDefaultValueSql("((0))");
                entity.Property(e => e.TotalPercepciones).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.PeriodoNomina)
                    .WithMany(p => p.Nominas)
                    .HasForeignKey(d => d.PeriodoNominaId)
                    .OnDelete(DeleteBehavior.Cascade) // Agregar Cascade
                    .HasConstraintName("FK__Nomina__PeriodoN__5CD6CB2B");

                entity.HasOne(d => d.Trabajador)
                    .WithMany(p => p.Nominas)
                    .HasForeignKey(d => d.TrabajadorId)
                    .OnDelete(DeleteBehavior.Cascade) // Agregar Cascade
                    .HasConstraintName("FK__Nomina__Trabajad__5BE2A6F2");
            });

            modelBuilder.Entity<Trabajador>(entity =>
            {
                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.TipoJornada)
                    .WithMany(p => p.Trabajadors)
                    .HasForeignKey(d => d.TipoJornadaId)
                    .OnDelete(DeleteBehavior.Restrict) // Opcional, depende de tu lógica
                    .HasConstraintName("FK__Trabajado__TipoJ__4D94879B");

                entity.HasOne(d => d.TipoSalario)
                    .WithMany(p => p.Trabajadors)
                    .HasForeignKey(d => d.TipoSalarioId)
                    .OnDelete(DeleteBehavior.Restrict) // Opcional, depende de tu lógica
                    .HasConstraintName("FK__Trabajado__TipoS__4E88ABD4");

                entity.HasOne(d => d.Periodicidad)
                    .WithMany()
                    .HasForeignKey(d => d.PeriodicidadId)
                    .OnDelete(DeleteBehavior.Restrict) // Opcional, depende de tu lógica
                    .HasConstraintName("FK_Trabajador_Periodicidad");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
