using System.ComponentModel.DataAnnotations;
namespace PusulaSu.Models;
public class SayacOkumasi
{
    public int Id { get; set; }
    public int AboneKaydiId { get; set; }
    public DateTime Tarih { get; set; }
        [Range(0, double.MaxValue, ErrorMessage 
        = "Endeks sıfırdan küçük olamaz.")]
    public decimal Endeks { get; set; }
    public decimal? ToplamBedel { get; set; } 
    public AboneKaydi? AboneKaydi { get; set; }
}