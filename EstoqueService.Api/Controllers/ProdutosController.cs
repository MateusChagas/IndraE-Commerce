using EstoqueService.Domain;
using EstoqueService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly EstoqueDbContext _db;


    public ProdutosController(EstoqueDbContext db)
    {
        _db = db;
    }


    // GET api/products
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Produto>>> GetAll()
    {
        return await _db.Produtos.AsNoTracking().ToListAsync();
    }


    // GET api/products/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Produto>> GetById(Guid id)
    {
        var product = await _db.Produtos.FindAsync(id);
        if (product == null) return NotFound();
        return product;
    }


    // POST api/products
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Produto>> Create(Produto product)
    {
        product.Id = Guid.NewGuid();
        _db.Produtos.Add(product);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }


    // PUT api/products/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, Produto updated)
    {
        var product = await _db.Produtos.FindAsync(id);
        if (product == null) return NotFound();


        product.Nome = updated.Nome;
        product.Descricao = updated.Descricao;
        product.Preco = updated.Preco;
        product.Quantidade = updated.Quantidade;


        await _db.SaveChangesAsync();
        return NoContent();
    }


    // DELETE api/products/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _db.Produtos.FindAsync(id);
        if (product == null) return NotFound();


        _db.Produtos.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}