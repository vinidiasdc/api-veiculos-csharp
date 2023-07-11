using apiVeiculos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace apiVeiculos.Infraestrutura
{
    public class ConexaoContext : DbContext
    {
        public ConexaoContext(DbContextOptions<ConexaoContext> options) : base(options) { }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ConexaoContext).Assembly);

            builder.Entity<Categoria>().HasKey(t => t.Id);
            builder.Entity<Categoria>().Property(p => p.Nome).HasMaxLength(30).IsRequired();

            // 1 : N -> Categoria - Veiculo
            builder.Entity<Categoria>()
                   .HasMany(p => p.Veiculos)
                   .WithOne(b => b.Categoria)
                   .HasForeignKey(b => b.CategoriaId);

            // Definição dos dados iniciais da tabela de categoria
            builder.Entity<Categoria>()
                .HasData(
                    new Categoria(1, "Rodoviário"),
                    new Categoria(2, "Ferroviário"),
                    new Categoria(3, "Aéreo"),
                    new Categoria(4, "Maritímo")
                );

            builder.Entity<Veiculo>().HasKey(t => t.Id);

            //configura a tabela da entidade
            builder.Entity<Veiculo>().Property(v => v.Nome).HasMaxLength(30).IsRequired();
            builder.Entity<Veiculo>().Property(v => v.Marca).HasMaxLength(30).IsRequired();
            builder.Entity<Veiculo>().Property(v => v.Ano).HasMaxLength(30).IsRequired();

            builder.Entity<Veiculo>().Property(v => v.Velocidade).HasPrecision(10,2);

            // Define o comportamento de exclusão de todas as chaves estrangeiras
            // no modelo de dados como ClientSetNull, para que a exclusão de uma
            // entidade relacionada resulte na definição dos valores das chaves
            // estrangeiras como null nas entidades referenciadas.
            foreach (var relationship in builder.Model.GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        }
    }
}
