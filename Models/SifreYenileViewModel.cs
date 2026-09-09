using System.ComponentModel.DataAnnotations;

namespace PusulaSu.Models;

public class SifreYenileViewModel
{
    [Required(ErrorMessage = "Abone numarası zorunludur.")]
    [Display(Name = "Abone Numarası")]
    public string AboneNo { get; set; } = "";

    [Required(ErrorMessage = "Sayaç numarası zorunludur.")]
    [Display(Name = "Sayaç Numarası")]
    public string SayacNo { get; set; } = "";

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "Şifre en az {2}, en fazla {1} karakter olmalıdır.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string YeniSifre { get; set; } = "";

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre Tekrarı")]
    [Compare(
        nameof(YeniSifre),
        ErrorMessage = "Şifreler eşleşmiyor.")]
    public string YeniSifreTekrari { get; set; } = "";
}