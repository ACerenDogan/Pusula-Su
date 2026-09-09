namespace PusulaSu.Models;
public class TuketimDonemi
{
    public int Id { get; set; }

    public int AboneKaydiId { get; set; }
    public AboneKaydi? AboneKaydi { get; set; }

    public int BaslangicOkumasiId { get; set; }
    public int BitisOkumasiId { get; set; }

    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }

    public decimal BaslangicEndeksi { get; set; }
    public decimal BitisEndeksi { get; set; }
    public decimal ToplamTuketim { get; set; }

    public decimal SuBedeli { get; set; }
    public decimal AtikSuBedeli { get; set; }
    public decimal SuKdv { get; set; }
    public decimal AtikSuKdv { get; set; }
    public decimal Ctv { get; set; }
    public decimal ToplamBedel { get; set; }

    public DateTime KapatilmaTarihi { get; set; }
    public bool IptalEdildi { get; set; } = false;
public DateTime? IptalTarihi { get; set; }
}