namespace Domain.DTOs.OrderDTO;

public class OrderDTO
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ProductId { get; set; }
    public DateTime OrderDate { get; set; }
}
