﻿namespace Rentacar.Models
{
    public abstract class Search
    {

        public int? AlisLokasyonId { get; set; }

        public int? TeslimLokasyonId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
