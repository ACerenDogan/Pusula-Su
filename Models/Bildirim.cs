namespace PusulaSu.Models;

public class Bildirim
{
    public int Id { get; set; }

    public string Baslik { get; set; } = "";

    public string Mesaj { get; set; } = "";

    public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

    public bool Aktif { get; set; } = true;
}