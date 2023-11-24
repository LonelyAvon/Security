namespace WpfApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Workers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdWorker { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WorkerCode { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [StringLength(30)]
        public string Patrynomic { get; set; }

        [StringLength(30)]
        public string Division { get; set; }

        [StringLength(30)]
        public string Depart { get; set; }

        [StringLength(50)]
        public string SecretWord { get; set; }
    }
}
