using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10140587_CLDV6212_POE.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // POST: Cart/AddToCart
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var product = await _context.Product.FindAsync(productId);
        if (product == null)
        {
            return NotFound();
        }

        // Check if the item is already in the cart
        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.ProductId == productId);

        if (existingCartItem != null)
        {
            // Update quantity if the item already exists in the cart
            existingCartItem.Quantity += quantity;
        }
        else
        {
            // Add new item to the cart
            var cartItem = new CartItem
            {
                ProductId = productId,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = quantity
            };

            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // GET: Cart/Index
    public async Task<IActionResult> Index()
    {
        var cartItems = await _context.CartItems.ToListAsync();
        return View(cartItems);
    }

    // POST: Cart/Remove
    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    // POST: Cart/Checkout
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        // Get the current user's ID and username
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;
        var userName = user.UserName;

        // Retrieve cart items for this user
        var cartItems = await _context.CartItems.ToListAsync();

        if (!cartItems.Any())
        {
            return RedirectToAction("Index", "Home"); // Redirect if the cart is empty
        }

        // Create a new order
        var order = new Order
        {
            UserId = userId,
            UserName = userName, // Save the username
            OrderDate = DateTime.Now,
            TotalAmount = cartItems.Sum(item => item.TotalPrice),
            OrderItems = cartItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        // Save the order
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Clear the cart
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        // Redirect to home page after checkout
        return RedirectToAction("Index", "Home");
    }

    // GET: Cart/TransactionHistory
    public async Task<IActionResult> TransactionHistory()
    {
        // Get the current user's ID and role
        var user = await _userManager.GetUserAsync(User);
        var userId = user.Id;
        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        List<Order> orders;

        if (isAdmin)
        {
            // Admins see all orders
            orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        else
        {
            // Regular users see only their own orders
            orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        return View(orders);
    }
}
