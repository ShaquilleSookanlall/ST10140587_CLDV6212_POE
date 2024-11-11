using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10140587_CLDV6212_POE.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Home/Index
    public async Task<IActionResult> Index()
    {
        var products = await _context.Product.ToListAsync();
        return View(products);
    }

    // GET: Home/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}
