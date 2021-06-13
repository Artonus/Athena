using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Athena.DataAccess.Model
{
    public class ProductRetailer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public long RetailerID { get; set; }
        public Retailer Retailer { get; set; }

        public long ProductID { get; set; }
        public Product Product { get; set; }
    }
}
