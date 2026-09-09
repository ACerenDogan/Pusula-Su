using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PusulaSu.Models;

namespace PusulaSu.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public readonly UserManager<IdentityUser> _userManager;

    public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
{
    var viewModel = await PanelModeliniHazirlaAsync();

    if (viewModel == null)
    {
        return NotFound();
    }

    return View(viewModel);
}

private async Task<DashboardViewModel?> PanelModeliniHazirlaAsync()
    {
      string? kullaniciId = _userManager.GetUserId(User);

        var aboneKaydi = await _context.AboneKayitlari
    .Include(a => a.SayacOkumalari)
    .Include(a => a.TuketimDonemleri)
    .FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);

        if (aboneKaydi == null)
        {
            return null;
        }

    aboneKaydi.SayacOkumalari = aboneKaydi.SayacOkumalari
    .OrderByDescending(o => o.Tarih)
    .ToList();
       var viewModel = new DashboardViewModel
    {
        Abone = aboneKaydi,
        
    };

    // Sayaç okumalarını eskiden yeniye doğru sıralar.
var siraliOkumalar = aboneKaydi.SayacOkumalari
    .OrderBy(o => o.Tarih)
    .ThenBy(o => o.Id)
    .ToList();

// Abonenin iptal edilmemiş son kapatılan dönemini bulur.
var sonKapatilanDonem = await _context.TuketimDonemleri
    .Where(d =>
        d.AboneKaydiId == aboneKaydi.Id &&
        !d.IptalEdildi)
    .OrderByDescending(d => d.BitisTarihi)
    .ThenByDescending(d => d.Id)
    .FirstOrDefaultAsync();

viewModel.SonKapatilanDonem = sonKapatilanDonem;

// Hiç dönem kapatılmadıysa ilk sayaç okumasından başlanır.
// Dönem kapatıldıysa kapatılan dönemin son okuması,
// yeni dönemin başlangıç okuması olur.
var baslangicOkumasi = sonKapatilanDonem == null
    ? siraliOkumalar.FirstOrDefault()
    : siraliOkumalar.FirstOrDefault(
        o => o.Id == sonKapatilanDonem.BitisOkumasiId);

var sonOkuma = siraliOkumalar.LastOrDefault();

if (baslangicOkumasi == null || sonOkuma == null)
{
    return viewModel;
}

viewModel.BaslangicOkumasiId = baslangicOkumasi.Id;
viewModel.BitisOkumasiId = sonOkuma.Id;
DateTime gorunenDonemBaslangici = sonKapatilanDonem == null
    ? baslangicOkumasi.Tarih.Date
    : sonKapatilanDonem.BitisTarihi.Date.AddDays(1);

viewModel.DonemBaslangicTarihi = gorunenDonemBaslangici;
viewModel.DonemBitisTarihi = sonOkuma.Tarih.Date;

// Başlangıç ve son okuma farklıysa açık dönemin tüketimini hesaplar.
if (baslangicOkumasi.Id != sonOkuma.Id)
{
    viewModel.AylikSuTuketimi =
        sonOkuma.Endeks - baslangicOkumasi.Endeks;

    viewModel.DonemKapatilabilir = true;
}

    // Abone türüne ait, yayın tarihi belirlenmiş bütün tarifeleri getirir.
var tumTarifeler = await _context.Tarifeler
    .Where(t =>
        t.AboneTuru == aboneKaydi.AboneTuru &&
        t.YayinTarihi.HasValue)
    .OrderBy(t => t.YayinTarihi)
    .ThenBy(t => t.AltSinir)
    .ToListAsync();

// Tarife, yayın tarihinden sonraki gün yürürlüğe girer.
var tarifeGruplari = tumTarifeler
    .GroupBy(t =>
        t.YayinTarihi.GetValueOrDefault().Date.AddDays(1))
    .OrderBy(g => g.Key)
    .ToList();

viewModel.TarifeDönemi =
    $"{gorunenDonemBaslangici:dd.MM.yyyy} - " +
    $"{sonOkuma.Tarih:dd.MM.yyyy}";

int toplamGunSayisi =
    (sonOkuma.Tarih.Date - baslangicOkumasi.Tarih.Date).Days;

if (toplamGunSayisi > 0 && viewModel.AylikSuTuketimi >= 0)
{
    var tarifeGunSayilari = new Dictionary<DateTime, int>();
    bool tarifesizGunVar = false;

    // Başlangıç okumasından sonraki gün ile bitiş günü arasını inceler.
    for (DateTime gun = baslangicOkumasi.Tarih.Date.AddDays(1);
         gun <= sonOkuma.Tarih.Date;
         gun = gun.AddDays(1))
    {
        var aktifTarife = tarifeGruplari
            .LastOrDefault(g => g.Key <= gun);

        if (aktifTarife == null)
        {
            tarifesizGunVar = true;
            break;
        }

        if (!tarifeGunSayilari.ContainsKey(aktifTarife.Key))
        {
            tarifeGunSayilari[aktifTarife.Key] = 0;
        }

        tarifeGunSayilari[aktifTarife.Key]++;
    }

    if (!tarifesizGunVar && tarifeGunSayilari.Count > 0)
    {
        viewModel.TarifeBulundu = true;

        var tarifeParcalari = tarifeGunSayilari
            .OrderBy(p => p.Key)
            .ToList();

        decimal dagitilanTuketim = 0;

        for (int i = 0; i < tarifeParcalari.Count; i++)
        {
            var parca = tarifeParcalari[i];

            // Son parçada, ondalık bölme farkını da içine alır.
            decimal parcaTuketimi =
                i == tarifeParcalari.Count - 1
                    ? viewModel.AylikSuTuketimi - dagitilanTuketim
                    : viewModel.AylikSuTuketimi *
                      parca.Value / toplamGunSayisi;

            dagitilanTuketim += parcaTuketimi;

            var kademeler = tarifeGruplari
                .First(g => g.Key == parca.Key)
                .ToList();

            var bedeller =
                KademeBedeliHesapla(parcaTuketimi, kademeler);

            viewModel.SuBedeli += bedeller.SuBedeli;
            viewModel.AtikSuBedeli += bedeller.AtikSuBedeli;
        }

        // Vergi hesapları
        viewModel.Ctv = viewModel.AylikSuTuketimi * 4.00m;
        viewModel.SuKdv = viewModel.SuBedeli * 0.01m;
        viewModel.AtikSuKdv = viewModel.AtikSuBedeli * 0.10m;

        viewModel.ToplamBedel =
            viewModel.SuBedeli +
            viewModel.AtikSuBedeli +
            viewModel.Ctv +
            viewModel.SuKdv +
            viewModel.AtikSuKdv;
    }
}
        return viewModel;
}
private static (decimal SuBedeli, decimal AtikSuBedeli) KademeBedeliHesapla(
    decimal tuketim,
    List<Tarife> tarifeler)
{
    decimal suBedeli = 0;
    decimal atikSuBedeli = 0;

    foreach (var tarife in tarifeler.OrderBy(t => t.AltSinir))
    {
        if (tuketim <= tarife.AltSinir)
        {
            continue;
        }

        decimal kademeSonu = tarife.UstSinir ?? tuketim;

        decimal kademedekiTuketim =
            Math.Min(tuketim, kademeSonu) - tarife.AltSinir;

        if (kademedekiTuketim <= 0)
        {
            continue;
        }

        suBedeli +=
            kademedekiTuketim * tarife.SuBirimFiyati;

        atikSuBedeli +=
            kademedekiTuketim * tarife.AtikSuBirimFiyati;
    }

    return (suBedeli, atikSuBedeli);
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> OkumaEkle(DateTime tarih, decimal endeks)
{
    string? kullaniciId = _userManager.GetUserId(User);

    var aboneKaydi = await _context.AboneKayitlari
        .FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);

    if (aboneKaydi == null)
    {
        return NotFound();
    }

   
    
tarih = tarih.Date;

if (tarih > DateTime.Today)
{
    TempData["Hata"] = "Gelecek bir tarih için sayaç okuması giremezsiniz.";
    return RedirectToAction(nameof(Index));
}

if (endeks < 0)
{
    TempData["Hata"] = "Sayaç endeksi negatif olamaz.";
    return RedirectToAction(nameof(Index));
}

DateTime ertesiGun = tarih.AddDays(1);

bool ayniTarihteOkumaVar = await _context.SayacOkumalari.AnyAsync(o =>
    o.AboneKaydiId == aboneKaydi.Id &&
    o.Tarih >= tarih &&
    o.Tarih < ertesiGun);

if (ayniTarihteOkumaVar)
{
    TempData["Hata"] = "Bu tarih için daha önce sayaç okuması girilmiş.";
    return RedirectToAction(nameof(Index));
}
var sonOkuma = await _context.SayacOkumalari
    .Where(o => o.AboneKaydiId == aboneKaydi.Id)
    .OrderByDescending(o => o.Tarih)
    .ThenByDescending(o => o.Id)
    .FirstOrDefaultAsync();

if (sonOkuma != null && tarih <= sonOkuma.Tarih)
{
    TempData["Hata"] =
        $"Yeni okuma tarihi {sonOkuma.Tarih:dd.MM.yyyy} tarihinden sonra olmalıdır.";

    return RedirectToAction(nameof(Index));
}

if (sonOkuma != null && endeks < sonOkuma.Endeks)
{
    TempData["Hata"] =
        $"Yeni endeks, son endeks olan {sonOkuma.Endeks:0.##} m³ değerinden küçük olamaz.";

    return RedirectToAction(nameof(Index));
}
        var yeniOkuma = new SayacOkumasi 
    {
        AboneKaydiId = aboneKaydi.Id,
        Tarih = tarih,
        Endeks = endeks
    };

    
   
    
    // --- YENİ EKLENEN KISIM BİTİŞ ---

    _context.SayacOkumalari.Add(yeniOkuma);
    await _context.SaveChangesAsync();
TempData["Basari"] = "Sayaç okuması başarıyla kaydedildi.";
    return RedirectToAction(nameof(Index));
    
}
 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SonOkumayiSil(int id)
{
    string? kullaniciId = _userManager.GetUserId(User);

    var aboneKaydi = await _context.AboneKayitlari
        .FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);

    if (aboneKaydi == null)
    {
        return NotFound();
    }

    var sonOkuma = await _context.SayacOkumalari
        .Where(o => o.AboneKaydiId == aboneKaydi.Id)
        .OrderByDescending(o => o.Tarih)
        .ThenByDescending(o => o.Id)
        .FirstOrDefaultAsync();

    if (sonOkuma == null || sonOkuma.Id != id)
    {
        TempData["Hata"] = "Yalnızca en son sayaç okuması silinebilir.";
        return RedirectToAction(nameof(Index));
    }

    _context.SayacOkumalari.Remove(sonOkuma);
    await _context.SaveChangesAsync();

    TempData["Basari"] =
        "Son sayaç okuması silindi. Doğru değeri yeniden girebilirsiniz.";

    return RedirectToAction(nameof(Index));
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DonemiKapat()
{
    var viewModel = await PanelModeliniHazirlaAsync();

    if (viewModel == null)
    {
        return NotFound();
    }

    if (!viewModel.DonemKapatilabilir ||
        !viewModel.TarifeBulundu ||
        !viewModel.BaslangicOkumasiId.HasValue ||
        !viewModel.BitisOkumasiId.HasValue)
    {
        TempData["Hata"] =
            "Kapatılabilecek hesaplanmış bir tüketim dönemi bulunamadı.";

        return RedirectToAction(nameof(Index));
    }

    bool zatenKapatildi = await _context.TuketimDonemleri.AnyAsync(d =>
        d.AboneKaydiId == viewModel.Abone.Id &&
        d.BaslangicOkumasiId == viewModel.BaslangicOkumasiId.Value &&
        d.BitisOkumasiId == viewModel.BitisOkumasiId.Value &&
        !d.IptalEdildi);

    if (zatenKapatildi)
    {
        TempData["Hata"] = "Bu tüketim dönemi zaten kapatılmış.";

        return RedirectToAction(nameof(Index));
    }

    var baslangicOkumasi = await _context.SayacOkumalari
        .FirstOrDefaultAsync(o =>
            o.Id == viewModel.BaslangicOkumasiId.Value &&
            o.AboneKaydiId == viewModel.Abone.Id);

    var bitisOkumasi = await _context.SayacOkumalari
        .FirstOrDefaultAsync(o =>
            o.Id == viewModel.BitisOkumasiId.Value &&
            o.AboneKaydiId == viewModel.Abone.Id);

    if (baslangicOkumasi == null || bitisOkumasi == null)
    {
        TempData["Hata"] = "Döneme ait sayaç okumaları bulunamadı.";

        return RedirectToAction(nameof(Index));
    }

    var yeniDonem = new TuketimDonemi
    {
        AboneKaydiId = viewModel.Abone.Id,

        BaslangicOkumasiId = baslangicOkumasi.Id,
        BitisOkumasiId = bitisOkumasi.Id,

        BaslangicTarihi = baslangicOkumasi.Tarih,
        BitisTarihi = bitisOkumasi.Tarih,

        BaslangicEndeksi = baslangicOkumasi.Endeks,
        BitisEndeksi = bitisOkumasi.Endeks,

        ToplamTuketim = viewModel.AylikSuTuketimi,

        SuBedeli = viewModel.SuBedeli,
        AtikSuBedeli = viewModel.AtikSuBedeli,
        Ctv = viewModel.Ctv,
        SuKdv = viewModel.SuKdv,
        AtikSuKdv = viewModel.AtikSuKdv,
        ToplamBedel = viewModel.ToplamBedel,

        KapatilmaTarihi = DateTime.Now,
        IptalEdildi = false
    };
bitisOkumasi.ToplamBedel = viewModel.ToplamBedel;
    _context.TuketimDonemleri.Add(yeniDonem);
    await _context.SaveChangesAsync();

    TempData["Basari"] =
        "Tüketim dönemi kapatıldı. Yeni dönem son endeksten başlayacak.";

    return RedirectToAction(nameof(Index));
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SonDonemiGeriAl()
{
    string? kullaniciId = _userManager.GetUserId(User);

    var aboneKaydi = await _context.AboneKayitlari
        .FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);

    if (aboneKaydi == null)
    {
        return NotFound();
    }

    var sonDonem = await _context.TuketimDonemleri
        .Where(d =>
            d.AboneKaydiId == aboneKaydi.Id &&
            !d.IptalEdildi)
        .OrderByDescending(d => d.KapatilmaTarihi)
        .ThenByDescending(d => d.Id)
        .FirstOrDefaultAsync();

    if (sonDonem == null)
    {
        TempData["Hata"] =
            "Geri alınabilecek kapatılmış bir dönem bulunamadı.";

        return RedirectToAction(nameof(Index));
    }

    sonDonem.IptalEdildi = true;
    sonDonem.IptalTarihi = DateTime.Now;

    await _context.SaveChangesAsync();

    TempData["Basari"] = "Son kapatılan dönem geri alındı.";

    return RedirectToAction(nameof(Index));
}
}