using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int MyProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Qty { get; set; }
        public string Flavor { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public double ShippingCharge { get; set; }
        public double Total { get; set; }
    }
}