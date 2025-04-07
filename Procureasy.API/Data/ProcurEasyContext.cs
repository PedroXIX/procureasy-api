using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Procureasy.API.Models;

namespace Procureasy.API.Data;

public partial class ProcurEasyContext : DbContext
{
    public ProcurEasyContext()
    {
    }

    public ProcurEasyContext(DbContextOptions<ProcurEasyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lance> Lances { get; set; }

    public virtual DbSet<Leilao> Leiloes { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lances__3214EC078C87B77A");

            entity.HasIndex(e => e.LeilaoId, "IX_Lance_LeilaoId");

            entity.HasIndex(e => e.UsuarioId, "IX_Lance_UsuarioId");

            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observacao)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Leilao).WithMany(p => p.Lances)
                .HasForeignKey(d => d.LeilaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lance_Leilao");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Lances)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lance_Usuario");
        });

        modelBuilder.Entity<Leilao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Leilaoes__3214EC0745BE6307");

            entity.HasIndex(e => e.DataTermino, "IX_Leilao_DataTermino");

            entity.HasIndex(e => e.Status, "IX_Leilao_Status");

            entity.Property(e => e.DataAtualizacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataEntrega).HasColumnType("datetime");
            entity.Property(e => e.DataInicio).HasColumnType("datetime");
            entity.Property(e => e.DataTermino).HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PrecoFinal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecoInicial).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ABERTO");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Produto).WithMany(p => p.Leiloes)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leilao_Produto");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Leiloes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leilao_Usuario");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Produtos__3214EC072A97E72E");

            entity.Property(e => e.Area)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataAtualizacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07375090E8");

            entity.HasIndex(e => e.Email, "IX_Usuario_Email");

            entity.HasIndex(e => e.TipoUsuario, "IX_Usuario_TipoUsuario");

            entity.HasIndex(e => e.Email, "UQ_Usuario_Email").IsUnique();

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Cnpj)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.DataCriacao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}