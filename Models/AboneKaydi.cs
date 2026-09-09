namespace PusulaSu.Models;

    public class AboneKaydi
    { 
        public int Id { get; set; } 
        public string AboneNo { get; set; } = "" ;
        public string SayacNo { get; set; } = "" ;
        public string AdSoyad { get; set; } = "" ;
        public string AboneTuru { get; set; } = "Mesken";
        public string? KullaniciId { get; set; }
        public List<SayacOkumasi> SayacOkumalari { get; set; } = new();
        public List<TuketimDonemi> TuketimDonemleri { get; set; } = new();
 }