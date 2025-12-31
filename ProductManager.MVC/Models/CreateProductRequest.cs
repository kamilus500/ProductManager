using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.MVC.Models
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }
    }
}
