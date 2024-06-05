namespace ImportExportCsvXls.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ProductModel()
        {
        }

        public ProductModel(string name, decimal price, bool active, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Active = active;
            Quantity = quantity;
        }
    }
}
