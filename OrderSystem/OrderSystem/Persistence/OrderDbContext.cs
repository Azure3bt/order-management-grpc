using Microsoft.EntityFrameworkCore;
namespace OrderSystem.Persistence;

public class OrderDbContext : DbContext
{
    public DbSet<OrderModels.Order> Orders { get; set; }
    public DbSet<OrderModels.Product> Products { get; set; }
    public DbSet<OrderModels.User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase(nameof(OrderService));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var orderModel = modelBuilder.Entity<OrderModels.Order>().HasNoKey();
        orderModel.Property(model => model.Id).ValueGeneratedOnAdd();
        orderModel.HasOne<OrderModels.Product>().WithOne("ProductId");
        orderModel.HasOne<OrderModels.User>().WithOne("UserId");

        var userModel = modelBuilder.Entity<OrderModels.User>().HasNoKey();
        userModel.Property(model => model.Id).ValueGeneratedOnAdd();
        userModel.HasMany<OrderModels.Order>().WithOne("UserId");

        var productModel = modelBuilder.Entity<OrderModels.Product>().HasNoKey();
        productModel.Property(model => model.Id).ValueGeneratedOnAdd();
        productModel.HasMany<OrderModels.Order>().WithOne("ProductId");
    }
}
