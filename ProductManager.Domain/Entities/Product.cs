namespace ProductManager.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public bool IsActive { get; private set; } = false;
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; } = false;


        //For ORM
        public Product()
        {

        }

        public Product(string name, decimal price, int quantity, bool isActive, string description)
        {
            Name = name;
            Price = price;
            StockQuantity = quantity;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }

        public void Activate(bool activation)
        {
            IsActive = activation;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void SetQuantity(int quantity)
        {
            StockQuantity = quantity;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}
