using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    [Column(TypeName = "decimal(18,2)")] // Specify precision and scale for Price
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;

    public Order Order { get; set; }
}
