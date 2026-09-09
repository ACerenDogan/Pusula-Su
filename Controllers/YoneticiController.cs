using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Models;
using Microsoft.AspNetCore.Mvc;
using PusulaSu.Data;

namespace PusulaSu.Controllers;

[Authorize(Roles = "Admin")]
public class YoneticiController : Controller
{
    private readonly ApplicationDbContext _context;

    public YoneticiController(ApplicationDbContext context)
    {
        _context = context;
    }
   public async Task<IActionResult> Index()
{
    DateTime bugun = DateTime.Today;

    int toplamAbone = await _context.AboneKayitlari.CountAsync();

    int kayitliKullanici = await _context.AboneKayitlari
        .CountAsync(a => a.KullaniciId != null);

    var guncelTarifeler = await _context.Tarifeler
        .Where(t => t.Yil == bugun.Year &&
                    t.Ay == bugun.Month)
        .ToListAsync();

    var viewModel = new YoneticiPaneliViewModel
    {
        ToplamAboneSayisi = toplamAbone,

        KayitliKullaniciSayisi = kayitliKullanici,

        HesapBekleyenAboneSayisi =
            toplamAbone - kayitliKullanici,

        KapatilanDonemSayisi = await _context.TuketimDonemleri
            .CountAsync(d => !d.IptalEdildi),

        GuncelTarifeDonemi =
            $"{bugun.Month:D2}/{bugun.Year}",

        GuncelTarifeKademeSayisi =
            guncelTarifeler.Count,

        SonTarifeYayinTarihi = guncelTarifeler
            .Where(t => t.YayinTarihi.HasValue)
            .OrderByDescending(t => t.YayinTarihi)
            .Select(t => t.YayinTarihi)
            .FirstOrDefault(),

        SonAboneler = await _context.AboneKayitlari
            .OrderByDescending(a => a.Id)
            .Take(5)
            .ToListAsync(),

        SonOkumalar = await _context.SayacOkumalari
            .Include(o => o.AboneKaydi)
            .OrderByDescending(o => o.Tarih)
            .ThenByDescending(o => o.Id)
            .Take(5)
            .ToListAsync()
    };

    return View(viewModel);
}
}