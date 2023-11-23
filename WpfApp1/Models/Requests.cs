namespace WpfApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Requests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Request { get; set; }

        public int ID_User { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [StringLength(30)]
        public string Patrynomic { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string DateOfBirth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IDSerial { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IDNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfVisit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WorkerCode { get; set; }

        public int? ID_Worker { get; set; }

        public int? GroupNumber { get; set; }

        public virtual Users Users { get; set; }

        public virtual Workers Workers { get; set; }
    }
}
