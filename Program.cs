using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProductDbContext>(options => options.UseInMemoryDatabase("Products"));
await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", (Func<string>)(() => "Hello World!"));

app.MapGet("/products", async (http) => {

var dbContext = http.RequestServices.GetService<ProductDbContext>();
var productItems = await dbContext.Products.ToListAsync();
await http.Response.WriteAsJsonAsync(productItems);

});

app.MapPost("/products", async(http) => {

var productItem = await http.Request.ReadFromJsonAsync<Product>();
var dbContext = http.RequestServices.GetService<ProductDbContext>();

await dbContext.Products.AddAsync(productItem);
await dbContext.SaveChangesAsync();

await http.Response.WriteAsJsonAsync("Ürün Eklendi");
});

app.MapPut("/products/{id}", async(http) => {
    if(!http.Request.RouteValues.TryGetValue("id",out var id)){
        http.Response.StatusCode=400;
        return;
    }
var dbContext = http.RequestServices.GetService<ProductDbContext>();
var productItem = await dbContext.Products.FindAsync(int.Parse(id.ToString()));

if(productItem ==null){
    http.Response.StatusCode = 404;
    return;
}

var inputProductItem = await http.Request.ReadFromJsonAsync<Product>();
productItem.UnitPrice = inputProductItem.UnitPrice;
productItem.Name = inputProductItem.Name;
await dbContext.SaveChangesAsync();
http.Response.StatusCode = 204;

});

app.MapDelete("/products/{id}", async (http) => {
      if(!http.Request.RouteValues.TryGetValue("id",out var id)){
        http.Response.StatusCode=400;
        return;
    }

    var dbContext = http.RequestServices.GetService<ProductDbContext>();
    var productItem = await dbContext.Products.FindAsync(int.Parse(id.ToString()));

    if(productItem == null){
          http.Response.StatusCode=404;
          await http.Response.WriteAsJsonAsync("Ürün Bulunamadı.");
        return;
    }

     dbContext.Products.Remove(productItem);
     await dbContext.SaveChangesAsync();

     http.Response.StatusCode = 204;
     await http.Response.WriteAsJsonAsync("Ürün Başarıyla Silindi");
    

});
await app.RunAsync();


public class ProductDbContext:DbContext{
    
    public ProductDbContext(DbContextOptions  context):base(context)
    {
        
    }

  public  DbSet<Product> Products{get;set;}
}

public class Product{
    public int Id { get; set; }
    public string Name { get; set; }
    public double UnitPrice { get; set; }
}
