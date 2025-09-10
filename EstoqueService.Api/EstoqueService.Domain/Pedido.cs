namespace IndraCommerce.EstoqueService.Api.EstoqueService.Domain
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataPedido { get; set; }
        public string? Status { get; set; }
        public decimal PrecoUnidade { get; set; }
    }
}
