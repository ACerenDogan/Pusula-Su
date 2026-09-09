using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Data;
using PusulaSu.Models;

namespace PusulaSu.Controllers;

public class SifreYenileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SifreYenileController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new SifreYenileViewModel());
    }
    [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Index(SifreYenileViewModel model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    string aboneNo = model.AboneNo.Trim();
    string sayacNo = model.SayacNo.Trim();

    var aboneKaydi = await _context.AboneKayitlari
        .FirstOrDefaultAsync(a =>
            a.AboneNo == aboneNo &&
            a.SayacNo == sayacNo);

    if (aboneKaydi == null ||
        string.IsNullOrWhiteSpace(aboneKaydi.KullaniciId))
    {
        ModelState.AddModelError(
            "",
            "Abone numarası veya sayaç numarası hatalıdır.");

        return View(model);
    }

    var kullanici = await _userManager.FindByIdAsync(
        aboneKaydi.KullaniciId);

    if (kullanici == null)
    {
        ModelState.AddModelError(
            "",
            "Bu aboneliğe bağlı kullanıcı hesabı bulunamadı.");

        return View(model);
    }

    string sifreYenilemeKodu =
        await _userManager.GeneratePasswordResetTokenAsync(kullanici);

    var sonuc = await _userManager.ResetPasswordAsync(
        kullanici,
        sifreYenilemeKodu,
        model.YeniSifre);

    if (!sonuc.Succeeded)
    {
        ModelState.AddModelError(
            "",
            "Şifre; büyük harf, küçük harf, rakam ve özel karakter içermelidir.");

        return View(model);
    }

    TempData["Basari"] =
        "Şifreniz başarıyla yenilendi. Yeni şifrenizle giriş yapabilirsiniz.";

    return RedirectToAction(nameof(Index));
}
}