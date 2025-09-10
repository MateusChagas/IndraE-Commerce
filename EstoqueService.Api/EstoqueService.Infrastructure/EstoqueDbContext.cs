using Microsoft.EntityFrameworkCore;

namespace EstoqueService.Infrastructure;

public class EstoqueDbContext:DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
    {
    }
    public DbSet<Domain.Produto> Produtos { get; set; }
}
