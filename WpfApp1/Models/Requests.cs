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
        public int IdRequest { get; set; }

        public int IdUser { get; set; }

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
        public decimal IdSerial { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IdNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfVisit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WorkerCode { get; set; }

        public int? IdWorker { get; set; }

        public int? GroupNumber { get; set; }

        public virtual Users Users { get; set; }
    }
}
