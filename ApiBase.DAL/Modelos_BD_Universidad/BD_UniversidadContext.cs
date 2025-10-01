using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class BD_UniversidadContext : DbContext
{
    public BD_UniversidadContext()
    {
    }

    public BD_UniversidadContext(DbContextOptions<BD_UniversidadContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Espacio> Espacios { get; set; }

    public virtual DbSet<FranjasHoraria> FranjasHorarias { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<GruposUsuario> GruposUsuarios { get; set; }

    public virtual DbSet<ObjetosPerdido> ObjetosPerdidos { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<ProyectosUsuario> ProyectosUsuarios { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TutoriasOferta> TutoriasOfertas { get; set; }

    public virtual DbSet<TutoriasOfertasTutoriasSolicitude> TutoriasOfertasTutoriasSolicitudes { get; set; }

    public virtual DbSet<TutoriasSolicitude> TutoriasSolicitudes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Espacio>(entity =>
        {
            entity.HasKey(e => e.idEspacio).HasName("Espacios_pkey");

            entity.Property(e => e.idEspacio).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.clasificacion).HasMaxLength(60);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
        });

        modelBuilder.Entity<FranjasHoraria>(entity =>
        {
            entity.HasKey(e => e.idFranjaHoraria).HasName("FranjasHorarias_pkey");

            entity.Property(e => e.idFranjaHoraria).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.etiqueta).HasMaxLength(20);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.idGrupo).HasName("Grupos_pkey");

            entity.Property(e => e.idGrupo).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grupos_Usuarios");
        });

        modelBuilder.Entity<GruposUsuario>(entity =>
        {
            entity.HasKey(e => e.idGruposUsuarios).HasName("GruposUsuarios_pkey");

            entity.HasIndex(e => new { e.idGrupo, e.idUsuario }, "UQ_GruposUsuarios").IsUnique();

            entity.Property(e => e.idGruposUsuarios).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
            entity.Property(e => e.rol)
                .HasMaxLength(20)
                .HasDefaultValueSql("'miembro'::character varying");
            entity.Property(e => e.unidoEn).HasDefaultValueSql("now()");

            entity.HasOne(d => d.idGrupoNavigation).WithMany(p => p.GruposUsuarios)
                .HasForeignKey(d => d.idGrupo)
                .HasConstraintName("FK_GU_Grupos");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.GruposUsuarios)
                .HasForeignKey(d => d.idUsuario)
                .HasConstraintName("FK_GU_Usuarios");
        });

        modelBuilder.Entity<ObjetosPerdido>(entity =>
        {
            entity.HasKey(e => e.idObjetoPerdido).HasName("ObjetosPerdidos_pkey");

            entity.Property(e => e.idObjetoPerdido).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying");
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.lugar).HasMaxLength(120);
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.ObjetosPerdidos)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ObjetosPerdidos_Usuarios");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.idProyecto).HasName("Proyectos_pkey");

            entity.HasIndex(e => e.nombre, "IX_Proyectos_Nombre");

            entity.Property(e => e.idProyecto).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.area).HasMaxLength(120);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.estado)
                .HasMaxLength(30)
                .HasDefaultValueSql("'en_curso'::character varying");
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proyectos_Usuarios");
        });

        modelBuilder.Entity<ProyectosUsuario>(entity =>
        {
            entity.HasKey(e => e.idProyectosUsuarios).HasName("ProyectosUsuarios_pkey");

            entity.HasIndex(e => new { e.idProyecto, e.idUsuario }, "UQ_ProyectosUsuarios").IsUnique();

            entity.Property(e => e.idProyectosUsuarios).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
            entity.Property(e => e.seguidoEn).HasDefaultValueSql("now()");

            entity.HasOne(d => d.idProyectoNavigation).WithMany(p => p.ProyectosUsuarios)
                .HasForeignKey(d => d.idProyecto)
                .HasConstraintName("FK_ProyUsu_Proyecto");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.ProyectosUsuarios)
                .HasForeignKey(d => d.idUsuario)
                .HasConstraintName("FK_ProyUsu_Usuario");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.idReserva).HasName("Reservas_pkey");

            entity.HasIndex(e => new { e.idEspacio, e.idFranjaHoraria, e.fecha }, "UQ_Reservas").IsUnique();

            entity.Property(e => e.idReserva).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'reservada'::character varying");
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idEspacioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.idEspacio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Espacios");

            entity.HasOne(d => d.idFranjaHorariaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.idFranjaHoraria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_FranjasHorarias");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Usuarios");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.idRol).HasName("Roles_pkey");

            entity.Property(e => e.idRol).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
        });

        modelBuilder.Entity<TutoriasOferta>(entity =>
        {
            entity.HasKey(e => e.idTutoriaOferta).HasName("TutoriasOfertas_pkey");

            entity.Property(e => e.idTutoriaOferta).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.cupo).HasDefaultValue((short)1);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.materia).HasMaxLength(120);
            entity.Property(e => e.modalidad)
                .HasMaxLength(30)
                .HasDefaultValueSql("'presencial'::character varying");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.TutoriasOferta)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutoriasOfertas_Usuarios");
        });

        modelBuilder.Entity<TutoriasOfertasTutoriasSolicitude>(entity =>
        {
            entity.HasKey(e => e.idTutoriasOfertasTutoriasSolicitudes).HasName("TutoriasOfertasTutoriasSolicitudes_pkey");

            entity.HasIndex(e => new { e.idTutoriaOferta, e.idTutoriaSolicitud }, "UQ_TutoriasOfertasTutoriasSolicitudes").IsUnique();

            entity.Property(e => e.idTutoriasOfertasTutoriasSolicitudes).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying");
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idTutoriaOfertaNavigation).WithMany(p => p.TutoriasOfertasTutoriasSolicitudes)
                .HasForeignKey(d => d.idTutoriaOferta)
                .HasConstraintName("FK_TOTs_Ofertas");

            entity.HasOne(d => d.idTutoriaSolicitudNavigation).WithMany(p => p.TutoriasOfertasTutoriasSolicitudes)
                .HasForeignKey(d => d.idTutoriaSolicitud)
                .HasConstraintName("FK_TOTs_Solicitudes");
        });

        modelBuilder.Entity<TutoriasSolicitude>(entity =>
        {
            entity.HasKey(e => e.idTutoriaSolicitud).HasName("TutoriasSolicitudes_pkey");

            entity.Property(e => e.idTutoriaSolicitud).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.materia).HasMaxLength(120);
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.TutoriasSolicitudes)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutoriasSolicitudes_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.idUsuario).HasName("Usuarios_pkey");

            entity.HasIndex(e => e.email, "IX_Usuarios_Email");

            entity.HasIndex(e => e.claveIdentidad, "UQ_Usuarios_ClaveIdentidad").IsUnique();

            entity.HasIndex(e => e.email, "UQ_Usuarios_Email").IsUnique();

            entity.HasIndex(e => e.userName, "UQ_Usuarios_userName").IsUnique();

            entity.Property(e => e.idUsuario).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.claveIdentidad).HasMaxLength(20);
            entity.Property(e => e.descripcion).HasMaxLength(200);
            entity.Property(e => e.email).HasMaxLength(150);
            entity.Property(e => e.fechaCreacion).HasDefaultValueSql("now()");
            entity.Property(e => e.motivoEliminacion).HasMaxLength(200);
            entity.Property(e => e.nombre).HasMaxLength(200);
            entity.Property(e => e.observaciones).HasMaxLength(200);
            entity.Property(e => e.passwordHash).HasMaxLength(255);
            entity.Property(e => e.primerApellido).HasMaxLength(200);
            entity.Property(e => e.segundoApellido).HasMaxLength(200);
            entity.Property(e => e.userName).HasMaxLength(30);

            entity.HasOne(d => d.idRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.idRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
