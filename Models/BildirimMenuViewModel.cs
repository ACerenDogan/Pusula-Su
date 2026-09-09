namespace PusulaSu.Models;

public class BildirimMenuViewModel
{
    public List<Bildirim> Bildirimler { get; set; } = new();

    public int OkunmamisSayisi { get; set; }
}