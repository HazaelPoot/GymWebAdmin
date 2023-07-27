using GymApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Infraestructure.Data
{
    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext()
        {
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {}

        public virtual DbSet<Horario> Horarios { get; set; }
        public virtual DbSet<Miembro> Miembros { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Gimnasio> Gimnasios { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<Suscripcion> Suscripcions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gimnasio>(entity =>
            {
                entity.HasKey(e => e.IdGimnasio)
                    .HasName("PK__Gimnasio__F11BDE4EA21B394C");

                entity.ToTable("Gimnasio");

                entity.Property(e => e.Ciudad)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Contacto)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Passw)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Horario>(entity =>
            {
                entity.HasKey(e => e.IdHorario)
                    .HasName("PK__Horario__1539229B897F64D1");

                entity.ToTable("Horario");

                entity.Property(e => e.DiaSemana)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.HoraFin)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.HoraInicio)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdGimnasioNavigation)
                    .WithMany(p => p.Horarios)
                    .HasForeignKey(d => d.IdGimnasio)
                    .HasConstraintName("FK__Horario__IdGimna__3B75D760");
            });

            modelBuilder.Entity<Miembro>(entity =>
            {
                entity.HasKey(e => e.IdMiembro)
                    .HasName("PK__Miembro__7B9226C8ECA07942");

                entity.ToTable("Miembro");

                entity.HasOne(d => d.IdGimnasioNavigation)
                    .WithMany(p => p.Miembros)
                    .HasForeignKey(d => d.IdGimnasio)
                    .HasConstraintName("FK__Miembro__IdGimna__74AE54BC");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Miembros)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Miembro__IdUsuar__73BA3083");
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasKey(e => e.IdServicio)
                    .HasName("PK__Servicio__2DCCF9A2D227CD4A");

                entity.ToTable("Servicio");

                entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Detalles).IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NombreFoto)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImagen)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGimnasioNavigation)
                    .WithMany(p => p.Servicios)
                    .HasForeignKey(d => d.IdGimnasio)
                    .HasConstraintName("FK__Servicio__IdGimn__38996AB5");
            });

            modelBuilder.Entity<Suscripcion>(entity =>
            {
                entity.HasKey(e => e.IdInscripcion)
                    .HasName("PK__Suscripc__A122F2BFEFF560CB");

                entity.ToTable("Suscripcion");

                entity.Property(e => e.ClasePago).HasMaxLength(50);

                entity.Property(e => e.ClaveAcceso).IsRequired();

                entity.HasOne(d => d.IdMiembroNavigation)
                    .WithMany(p => p.Suscripcions)
                    .HasForeignKey(d => d.IdMiembro)
                    .HasConstraintName("FK__Suscripci__IdMie__7C4F7684");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.Suscripcions)
                    .HasForeignKey(d => d.IdServicio)
                    .HasConstraintName("FK__Suscripci__IdSer__7D439ABD");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF97D905E896");

                entity.ToTable("Usuario");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Contacto)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Correo).IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Passw).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}