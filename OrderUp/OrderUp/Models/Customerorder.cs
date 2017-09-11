namespace OrderUp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CUSTOMERORDER")]
    public partial class CUSTOMERORDER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public string OrderDate { get; set; }

        [Required]
        [StringLength(25)]
        public string ServerName { get; set; }

        public decimal Subtotal { get; set; }

        [StringLength(25)]
        public string DiscountName { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal Pretax { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }
    }
}
