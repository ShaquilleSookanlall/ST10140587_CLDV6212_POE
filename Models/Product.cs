using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProductName { get; set; }

    [StringLength(500)]
    public string ProductDescription { get; set; }

    [Url]
    public string ImageUrl { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")] // Specify precision and scale for Price
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
