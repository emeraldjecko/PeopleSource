namespace PeoplesSource.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SellerInfo")]
    public partial class SellerInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public decimal? Increment { get; set; }

        public bool IsPercentage { get; set; }

        public decimal? KZ { get; set; }

        public decimal? OHT { get; set; }
    }
}
