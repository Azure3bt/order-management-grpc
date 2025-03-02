using Microsoft.EntityFrameworkCore;
using OrderModels.School;
namespace OrderSystem.Persistence;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<OrderModels.Order> Orders { get; set; }
    public DbSet<OrderModels.Product> Products { get; set; }
    public DbSet<OrderModels.User> Users { get; set; }
    
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var orderModel = modelBuilder.Entity<OrderModels.Order>();
        orderModel.HasKey(e => e.Id);
        orderModel.Property(e => e.Id).ValueGeneratedOnAdd();

        var userModel = modelBuilder.Entity<OrderModels.User>();
        userModel.HasKey(e => e.Id);
        userModel.Property(e => e.Id).ValueGeneratedOnAdd();
        userModel.HasData(
            new OrderModels.User(1, "User 1", "user1@mail.com"),
            new OrderModels.User(2, "User 2", "user2@mail.com"),
            new OrderModels.User(3, "User 3", "user3@mail.com")
        );

        var productModel = modelBuilder.Entity<OrderModels.Product>();
        productModel.HasKey(e => e.Id);
        productModel.Property(e => e.Id).ValueGeneratedOnAdd();
        productModel.HasData(
            new OrderModels.Product(1, "Product 1", 100),
            new OrderModels.Product(2, "Product 2", 200),
            new OrderModels.Product(3, "Product 3", 300)
        );

        var studentModel = modelBuilder.Entity<Student>();
        studentModel.HasKey(e => e.Id);
        studentModel.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
