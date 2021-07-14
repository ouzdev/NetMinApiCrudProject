using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

[Route("/api/[controller]")]
public class HomeController:Controller{
    [HttpGet]
    public IEnumerable<string> Get(){
    return new string[] { "value1", "value2" };
    }
}