namespace OrderUp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DISCOUNT")]
    public partial class DISCOUNT
    {
        public int DiscountID { get; set; }

        [Required]
        [StringLength(25)]
        public string DiscountName { get; set; }

        [Required]
        [StringLength(20)]
        public string DiscountType { get; set; }

        [Range(1,100)]
        public decimal DiscountAmount { get; set; }
    }
}
