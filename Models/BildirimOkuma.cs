namespace PusulaSu.Models;

public class BildirimOkuma
{
    public int Id { get; set; }

    public int BildirimId { get; set; }

    public Bildirim? Bildirim { get; set; }

    public string KullaniciId { get; set; } = "";

    public DateTime OkunmaTarihi { get; set; } = DateTime.Now;
}