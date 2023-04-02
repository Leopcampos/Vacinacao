using Fiotec.Vaicinacao.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiotec.Vaicinacao.API.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        { }

        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Solicitante>(entity =>
            {
                entity.ToTable("Solicitante");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.DataCriacao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Relatorio>(entity =>
            {
                entity.ToTable("Relatorio");

                entity.Property(e => e.DescricaoRelatorio)
                    .HasColumnName("Descricao")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DataSolicitacao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataAplicacao).HasColumnType("date");

                entity.Property(e => e.QuantidadeTotalDeVacinas)
                    .HasColumnName("QuantidadeTotalVacinas")
                    .IsRequired();

                entity.Property(e => e.SolicitanteId).HasColumnName("SolicitanteID");

                entity.HasOne(d => d.Solicitante)
                    .WithMany(p => p.Relatorios)
                    .HasForeignKey(d => d.SolicitanteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Relatorio_Usuario");
            });
        }
    }
}