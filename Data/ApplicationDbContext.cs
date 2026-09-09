using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PusulaSu.Models;

namespace PusulaSu.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<AboneKaydi> AboneKayitlari => Set<AboneKaydi>();
    public DbSet<SayacOkumasi> SayacOkumalari => Set<SayacOkumasi>();
    public DbSet<Tarife> Tarifeler => Set<Tarife>();
    public DbSet<Bildirim> Bildirimler { get; set; }
    public DbSet<BildirimOkuma> BildirimOkumalari { get; set; }
    public DbSet<TuketimDonemi> TuketimDonemleri { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BildirimOkuma>()
    .HasIndex(o => new
    {
        o.BildirimId,
        o.KullaniciId
    })
    .IsUnique();
        modelBuilder.Entity<AboneKaydi>().HasData(
            new AboneKaydi { Id = 1, AboneNo = "123456", SayacNo = "789012", AdSoyad = "Ceren Doğan" },
            new AboneKaydi { Id = 2, AboneNo = "654321", SayacNo = "210987", AdSoyad = "Zeren Nas"},
            new AboneKaydi { Id = 3, AboneNo = "987654", SayacNo = "345678", AdSoyad = "Ayşe Ceren"}
        );
        modelBuilder.Entity<Tarife>().HasData(
    new Tarife
    {
        Id = 1,
        Yil = 2026,
        Ay = 8,
        AboneTuru = "Mesken",
        AltSinir = 0,
        UstSinir = 15,
        SuBirimFiyati = 39.19m,
        AtikSuBirimFiyati = 17.64m
    },
    new Tarife
    {
        Id = 2,
        Yil = 2026,
        Ay = 8,
        AboneTuru = "Mesken",
        AltSinir = 15,
        UstSinir = 30,
        SuBirimFiyati = 59.08m,
        AtikSuBirimFiyati = 26.61m
    },
    new Tarife
    {
        Id = 3,
        Yil = 2026,
        Ay = 8,
        AboneTuru = "Mesken",
        AltSinir = 30,
        UstSinir = 75,
        SuBirimFiyati = 88.57m,
        AtikSuBirimFiyati = 39.85m
    },
    new Tarife
    {
        Id = 4,
        Yil = 2026,
        Ay = 8,
        AboneTuru = "Mesken",
        AltSinir = 75,
        UstSinir = null,
        SuBirimFiyati = 132.97m,
        AtikSuBirimFiyati = 59.81m
    }
);
    }

}
