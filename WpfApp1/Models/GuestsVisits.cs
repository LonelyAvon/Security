namespace WpfApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GuestsVisits
    {
        [Key]
        public int IdVisit { get; set; }

        public int? IdUser { get; set; }

        public bool? FirstPass { get; set; }

        public bool? SecondPass { get; set; }

        public bool? FinalPass { get; set; }

        public DateTime? EntranceDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public virtual Users Users { get; set; }
    }
}
