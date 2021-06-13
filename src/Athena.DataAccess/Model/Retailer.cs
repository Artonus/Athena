using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Athena.DataAccess.Model
{
    public class Retailer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(600)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Url { get; set; }
        public ICollection<ProductRetailer> ProductRetailers { get; set; }
    }
}