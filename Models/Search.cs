using System.ComponentModel.DataAnnotations.Schema;

namespace Rentacar.Models
{
    [NotMapped]
    public  class Search
    {

        public int? AlisLokasyonId { get; set; }

        public int? TeslimLokasyonId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? FinishTime { get; set; }
    }
}
