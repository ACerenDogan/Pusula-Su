using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Data;
using PusulaSu.Models;

namespace PusulaSu.ViewComponents;

public class BildirimViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public BildirimViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string? kullaniciId = ViewContext.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        DateTime yediGunOnce = DateTime.Now.AddDays(-7);

        var bildirimler = await _context.Bildirimler
            .Where(b => b.Aktif &&
                        b.OlusturmaTarihi >= yediGunOnce)
            .OrderByDescending(b => b.OlusturmaTarihi)
            .Take(5)
            .ToListAsync();

        int okunmamisSayisi = 0;

        if (!string.IsNullOrEmpty(kullaniciId))
        {
            var okunmusBildirimIdleri =
                await _context.BildirimOkumalari
                    .Where(o => o.KullaniciId == kullaniciId)
                    .Select(o => o.BildirimId)
                    .ToListAsync();

            okunmamisSayisi = bildirimler.Count(
                b => !okunmusBildirimIdleri.Contains(b.Id));
        }

        var model = new BildirimMenuViewModel
        {
            Bildirimler = bildirimler,
            OkunmamisSayisi = okunmamisSayisi
        };

        return View(model);
    }
}