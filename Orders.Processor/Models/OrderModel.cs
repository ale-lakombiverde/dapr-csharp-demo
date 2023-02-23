public class OrderModel
{
    public Guid OrderId { get; set; } = Guid.NewGuid();
    public string Reference { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime CreatedOn { get; set; }
}