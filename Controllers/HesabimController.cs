using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusulaSu.Data;
using Microsoft.EntityFrameworkCore;

namespace PusulaSu.Controllers;

[Authorize]
public class HesabimController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;



public HesabimController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
{
    string? kullaniciId = _userManager.GetUserId(User);

    var aboneKaydi = await _context.AboneKayitlari
        .FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);

    if (aboneKaydi == null)
    {
        return NotFound();
    }

    return View(aboneKaydi);
}
 }