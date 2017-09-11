namespace OrderUp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MENU")]
    public partial class MENU
    {
        public int MenuID { get; set; }

        [Required]
        [StringLength(25)]
        public string FoodName { get; set; }

        [Range(1,100)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(30)]
        public string Category { get; set; }
    }
}
