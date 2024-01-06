using HomebrewDesigner.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomebrewDesigner.Controllers;

[Route("[controller]/[action]")]
public class CalculatorController : Controller
{
    // GET
    public IActionResult Index()
    {
        CalculatorVM calc = new CalculatorVM();

        calc.OG = 1.050;
        calc.FG = 1.010;
        calc.ABV = Math.Round((calc.OG - calc.FG) * 131.25, 1);
        
        
        return View(calc);
    }
    
    [HttpPost]
    public IActionResult Index(CalculatorVM calc)
    {
        calc.ABV = Math.Round((calc.OG - calc.FG) * 131.25, 1);

        if (ModelState.IsValid)
        {
            return View(calc);
        }
        
        return View(calc);
        
    }
}