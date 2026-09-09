using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Data;
using PusulaSu.Models;

namespace PusulaSu.Controllers;

[Authorize(Roles = "Admin")]
public class TarifeYonetimiController : Controller
{
    private readonly ApplicationDbContext _context;

    public TarifeYonetimiController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        // En güncel yıl/ay en üstte olacak şekilde, ardından abone türü ve kademeye göre sıralıyoruz
        var tarifeler = await _context.Tarifeler
            .OrderByDescending(t => t.Yil)
            .ThenByDescending(t => t.Ay)
            .ThenBy(t => t.AboneTuru)
            .ThenBy(t => t.AltSinir)
            .ToListAsync();
        return View(tarifeler);
    }
    public async Task<IActionResult> Duzenle(int id)
{
    var tarife= await _context.Tarifeler 
    .FirstOrDefaultAsync(t => t.Id == id);
    if (tarife == null)
{
    return NotFound();
}

return View(tarife);
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Duzenle(Tarife gelenTarife)
{
    var mevcutTarife = await _context.Tarifeler
        .FirstOrDefaultAsync(t => t.Id == gelenTarife.Id);

    if (mevcutTarife == null)
    {
        return NotFound();
    }

    mevcutTarife.Yil = gelenTarife.Yil;
    mevcutTarife.Ay = gelenTarife.Ay;
    mevcutTarife.YayinTarihi = gelenTarife.YayinTarihi;
    mevcutTarife.AboneTuru = gelenTarife.AboneTuru;
    mevcutTarife.AltSinir = gelenTarife.AltSinir;
    mevcutTarife.UstSinir = gelenTarife.UstSinir;
    mevcutTarife.SuBirimFiyati = gelenTarife.SuBirimFiyati;
    mevcutTarife.AtikSuBirimFiyati = gelenTarife.AtikSuBirimFiyati;

   _context.Bildirimler.Add(new Bildirim
{
    Baslik = "Tarife Güncellendi",
    Mesaj = $"{mevcutTarife.Ay}/{mevcutTarife.Yil} dönemi " +
            $"{mevcutTarife.AboneTuru} tarifesi güncellendi.",
    OlusturmaTarihi = DateTime.Now,
    Aktif = true
});

await _context.SaveChangesAsync();

    TempData["Basari"] = "Tarife başarıyla güncellendi.";

    return RedirectToAction(nameof(Index));
    
}
public IActionResult Ekle()
{
    return View(new Tarife
    { Yil = DateTime.Today.Year,
        Ay = DateTime.Today.Month,
        AboneTuru = "Mesken"
         });
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Ekle(Tarife yeniTarife)
{
    if (yeniTarife.Ay < 1 || yeniTarife.Ay > 12)
    {
        ModelState.AddModelError("", "Ay 1 ile 12 arasında olmalıdır.");
    }

    if (yeniTarife.UstSinir.HasValue &&
        yeniTarife.UstSinir <= yeniTarife.AltSinir)
    {
        ModelState.AddModelError(
            "",
            "Üst sınır, alt sınırdan büyük olmalıdır.");
    }

    if (yeniTarife.SuBirimFiyati < 0 ||
        yeniTarife.AtikSuBirimFiyati < 0)
    {
        ModelState.AddModelError(
            "",
            "Birim fiyatları negatif olamaz.");
    }

    bool ayniTarifeVar = await _context.Tarifeler.AnyAsync(t =>
        t.Yil == yeniTarife.Yil &&
        t.Ay == yeniTarife.Ay &&
        t.AboneTuru == yeniTarife.AboneTuru &&
        t.AltSinir == yeniTarife.AltSinir);

    if (ayniTarifeVar)
    {
        ModelState.AddModelError(
            "",
            "Bu dönem ve kademe için tarife zaten bulunmaktadır.");
    }

    if (!ModelState.IsValid)
    {
        return View(yeniTarife);
    }

    yeniTarife.AboneTuru = yeniTarife.AboneTuru.Trim();

    _context.Tarifeler.Add(yeniTarife);
    _context.Bildirimler.Add(new Bildirim
{
    Baslik = "Yeni Tarife Yayınlandı",
    Mesaj = $"{yeniTarife.Ay}/{yeniTarife.Yil} dönemi " +
            $"{yeniTarife.AboneTuru} tarifesi yayınlandı.",
    OlusturmaTarihi = DateTime.Now,
    Aktif = true
});
    await _context.SaveChangesAsync();

    TempData["Basari"] = "Yeni tarife başarıyla eklendi.";

    return RedirectToAction(nameof(Index));

}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Sil(int id)
{
    var tarife = await _context.Tarifeler
        .FirstOrDefaultAsync(t => t.Id == id);

    if (tarife == null)
    {
        return NotFound();
    }

    _context.Tarifeler.Remove(tarife);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
}