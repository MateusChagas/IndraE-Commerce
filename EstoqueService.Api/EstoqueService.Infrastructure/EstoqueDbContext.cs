using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Infrastructure;

public class EstoqueDbContext:DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
    {
    }
    public DbSet<Domain.Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Domain.Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.Quantidade).IsRequired();
            entity.Property(e => e.Preco).IsRequired().HasColumnType("decimal(18,2)");
        });
    }
}
