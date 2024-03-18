namespace Asmmvc1670.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Country { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
