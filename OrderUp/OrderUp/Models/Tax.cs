namespace OrderUp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAX")]
    public partial class TAX
    {
        public int TaxID { get; set; }

        [Required]
        [StringLength(25)]
        public string TaxName { get; set; }

        [Range(1,100)]
        public int TaxAmount { get; set; }
    }
}
