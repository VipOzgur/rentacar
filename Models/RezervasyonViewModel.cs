namespace Rentacar.Models
{
    public class RezervasyonViewModel
    {
        public int UserId {  get; set; }
        public int AracId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int AlisLokasyonId { get; set; }
        public int TeslimLokasyonId { get; set; }
        public int KaskoId { get; set; }
        public int Fiyat { get; set; }
        public List<int> Extralar { get; set; } = new();
    }
}
