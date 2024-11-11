using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: Admin/UserList
    public async Task<IActionResult> UserList()
    {
        // Retrieve all users
        var users = await _userManager.Users.ToListAsync();

        // Return the view with the user list
        return View(users);
    }
}
