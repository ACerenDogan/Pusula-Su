namespace PusulaSu.Models;

public class YoneticiPaneliViewModel
{
    // Abone istatistikleri
    public int ToplamAboneSayisi { get; set; }
    public int KayitliKullaniciSayisi { get; set; }
    public int HesapBekleyenAboneSayisi { get; set; }

    // Tüketim dönemi istatistiği
    public int KapatilanDonemSayisi { get; set; }

    // Güncel tarife özeti
    public string GuncelTarifeDonemi { get; set; } = "-";
    public int GuncelTarifeKademeSayisi { get; set; }
    public DateTime? SonTarifeYayinTarihi { get; set; }

    // Yönetici ekranındaki son hareketler
    public List<AboneKaydi> SonAboneler { get; set; } = new();
    public List<SayacOkumasi> SonOkumalar { get; set; } = new();
}