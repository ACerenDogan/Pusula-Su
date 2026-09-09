using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Data;
using PusulaSu.Models;

namespace PusulaSu.Controllers;

[Authorize]
public class BildirimController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BildirimController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TumunuOkunduYap()
    {
        string? kullaniciId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(kullaniciId))
        {
            return Unauthorized();
        }

        DateTime yediGunOnce = DateTime.Now.AddDays(-7);

        var bildirimIdleri = await _context.Bildirimler
            .Where(b => b.Aktif &&
                        b.OlusturmaTarihi >= yediGunOnce)
            .OrderByDescending(b => b.OlusturmaTarihi)
            .Take(5)
            .Select(b => b.Id)
            .ToListAsync();

        var okunmusBildirimIdleri =
            await _context.BildirimOkumalari
                .Where(o => o.KullaniciId == kullaniciId)
                .Select(o => o.BildirimId)
                .ToListAsync();

        var yeniOkumalar = bildirimIdleri
            .Except(okunmusBildirimIdleri)
            .Select(bildirimId => new BildirimOkuma
            {
                BildirimId = bildirimId,
                KullaniciId = kullaniciId,
                OkunmaTarihi = DateTime.Now
            })
            .ToList();

        if (yeniOkumalar.Count > 0)
        {
            _context.BildirimOkumalari.AddRange(yeniOkumalar);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}