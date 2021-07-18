using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class ProductsController:Controller
{
    private readonly ProductDbContext _context;
    public ProductsController(ProductDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts(){
        return await _context.Products.ToListAsync();
    }

    [HttpPost]
    public async void AddProduct(ProductDto model){
        await _context.Products.AddAsync(new Product{Name = model.Name,UnitPrice = model.UnitPrice});
        await _context.SaveChangesAsync();
    }

    [HttpPut("{id}")]
    public async void UpdateProduct(int id ,ProductDto model)
    {
      var result =  await _context.Products.FindAsync(id);
      result.Name = model.Name;
      result.UnitPrice = model.UnitPrice;
      await _context.SaveChangesAsync();
    }
    [HttpDelete("{id}")]
    public async void DeleteProduct(int id){
        var result = await _context.Products.FindAsync(id);
         _context.Remove(result);
        await _context.SaveChangesAsync();

    }
}