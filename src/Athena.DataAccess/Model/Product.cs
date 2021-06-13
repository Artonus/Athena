using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Athena.DataAccess.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(600)]
        public string Description { get; set; }
        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(4000)]
        public string AccessTemplate { get; set; }

        public ICollection<ProductRetailer> ProductRetailers { get; set; }
    }
}