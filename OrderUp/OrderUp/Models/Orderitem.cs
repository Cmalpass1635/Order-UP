namespace OrderUp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDERITEM")]
    public partial class ORDERITEM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailID { get; set; }

        public int? OrderID { get; set; }

        [StringLength(25)]
        public string FoodName { get; set; }

        public decimal? FoodPrice { get; set; }
    }
}
