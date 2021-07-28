using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudy.DAL.DomainClasses
{
    public class Product
    {
        //Id string
        //    brand brand
        //    timer byte[]
        //    productName string
        //    grahpicname string 
        //    costprice decimal
        //    msrp decimal
        //    qtyonhand int
        //    qtyonbackorder int
        //    descripition string

        [Key]
        public string Id { get; set; }
        public Brand Brand { get; set; }
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }
        public string ProductName { get; set; }
        public string GraphicName { get; set; }

        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyOnBackOrder { get; set; }

        [MaxLength(2000)]
        [MinLength(50)]
        public string Description { get; set; }
    }
}
