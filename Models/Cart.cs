namespace Asmmvc1670.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total => Quantity * Price;
        public string? Image { get; set; }
        public string? Address { get; set; } 

        public Cart()
        {

        }

        public Cart(Product product)
        {
            ProductId = product.ProductId;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1;
            Image = product.Image;
            Address = " "; 
        }
    }
}
