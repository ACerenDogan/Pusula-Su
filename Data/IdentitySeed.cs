using Microsoft.AspNetCore.Identity;

namespace PusulaSu.Data;

public static class IdentitySeed
{
    public static async Task AdminOlusturAsync(
        IServiceProvider serviceProvider)
    {
        var userManager =
            serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRolu = "Admin";
        const string adminKullaniciAdi = "Admin";
        const string adminSifresi = "Admin123!";

        if (!await roleManager.RoleExistsAsync(adminRolu))
        {
            var rolSonucu =
                await roleManager.CreateAsync(new IdentityRole(adminRolu));

            if (!rolSonucu.Succeeded)
            {
                throw new Exception(
                    $"Admin rolü oluşturulamadı: {HatalariGetir(rolSonucu)}");
            }
        }

        var admin =
            await userManager.FindByNameAsync(adminKullaniciAdi);

        if (admin is null)
        {
            admin = new IdentityUser
            {
                UserName = adminKullaniciAdi
            };

            var kullaniciSonucu =
                await userManager.CreateAsync(admin, adminSifresi);

            if (!kullaniciSonucu.Succeeded)
            {
                throw new Exception(
                    $"Admin kullanıcısı oluşturulamadı: " +
                    $"{HatalariGetir(kullaniciSonucu)}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, adminRolu))
        {
            var rolAtamaSonucu =
                await userManager.AddToRoleAsync(admin, adminRolu);

            if (!rolAtamaSonucu.Succeeded)
            {
                throw new Exception(
                    $"Admin rolü kullanıcıya atanamadı: " +
                    $"{HatalariGetir(rolAtamaSonucu)}");
            }
        }
    }

    private static string HatalariGetir(IdentityResult sonuc)
    {
        return string.Join(
            ", ",
            sonuc.Errors.Select(hata => hata.Description));
    }
}