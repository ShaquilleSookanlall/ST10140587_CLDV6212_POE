using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }

    [Column(TypeName = "decimal(18,2)")] // Specify precision and scale
    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
