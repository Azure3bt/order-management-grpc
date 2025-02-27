namespace OrderModels;
public record Order
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public double Amount { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public OrderState State { get; set; } = OrderState.Created;
}
