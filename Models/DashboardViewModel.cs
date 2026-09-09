namespace PusulaSu.Models;
public class DashboardViewModel
{
    public AboneKaydi Abone { get; set; } = new();
    public decimal AylikSuTuketimi { get; set; }
    public decimal SuBedeli { get; set; }
    public decimal AtikSuBedeli { get; set; }
    public decimal ToplamBedel { get; set; }
    public string TarifeDönemi { get; set; } = "";
    public bool TarifeBulundu { get; set; }
    // Vergi ve ek ücretler
public decimal Ctv { get; set; }
public decimal SuKdv { get; set; }
public decimal AtikSuKdv { get; set; }

// Açık tüketim döneminin bilgileri
public int? BaslangicOkumasiId { get; set; }
public int? BitisOkumasiId { get; set; }

public DateTime? DonemBaslangicTarihi { get; set; }
public DateTime? DonemBitisTarihi { get; set; }

// Dönemi kapatma işleminin yapılıp yapılamayacağını belirtir
public bool DonemKapatilabilir { get; set; }

// Geri alınabilecek son kapatılmış dönem
public TuketimDonemi? SonKapatilanDonem { get; set; }

}