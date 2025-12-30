namespace ProductManager.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; } = false;
        public DateTime CreatedAt { get; private set; }


        //For ORM
        public Product()
        {

        }

        public Product(string name, decimal price, int quantity, bool isActive)
        {
            Name = name;
            Price = price;
            StockQuantity = quantity;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public void Activate(bool activation)
        {
            IsActive = activation;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void DecreaseQuantity(int quantity)
        {
            StockQuantity -= quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            StockQuantity += quantity;
        }
    }
}
