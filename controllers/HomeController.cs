using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly ProductDbContext _context;
    public HomeController(ProductDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Product> Get(){
        return _context.Products.ToList();
    }

    [HttpPost]
    public void AddProduct([FromForm] AddProductDTO model)
    {
        Product p = new(){
            Name = model.Name,
            UnitPrice = model.UnitPrice
        };
        _context.Products.Add(p);
        _context.SaveChanges();
    }

    [HttpPut("{id}")]
     public void UpdateProduct([FromRoute] int id, [FromForm] AddProductDTO model)
    {
       var updateProduct = _context.Products.Find(id);
       updateProduct.Name =model.Name;
       updateProduct.UnitPrice = model.UnitPrice;
        _context.SaveChanges(); 
    }

    [HttpDelete("{id}")]
    public void DeleteProduct([FromRoute] int id)
     {
       var deleteProduct = _context.Products.Find(id);
        _context.Products.Remove(deleteProduct);
        _context.SaveChanges(); 
    }
}
