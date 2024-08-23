using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjetoNoticiaV1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjetoNoticiaV1;

public partial class DbNoticiaContext : DbContext
{
    public DbNoticiaContext()
    {
    }

    public DbNoticiaContext(DbContextOptions<DbNoticiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NoticiaTag> NoticiaTags { get; set; }

    public virtual DbSet<Noticia> Noticia { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<NoticiaTag>(entity =>
        {
            entity.ToTable("NoticiaTag");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Noticia).WithMany(p => p.NoticiaTags)
                .HasForeignKey(d => d.NoticiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoticiaTag_Noticia");

            entity.HasOne(d => d.Tag).WithMany(p => p.NoticiaTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoticiaTag_Tag");
        });

        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Texto).HasColumnType("text");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Noticia)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Noticia_Usuario");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Nome).HasMaxLength(250);
            entity.Property(e => e.Senha).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
