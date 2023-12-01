namespace WpfApp1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WorkersPhoto")]
    public partial class WorkersPhoto
    {
        [Key]
        public int IdWorker { get; set; }

        public byte[] Data { get; set; }
    }
}
