
using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using onine.core.Entites;
using onine.core.Enums;
using onine.core.Repositories;
using onine.Repository.Data;
using onine.services;

namespace onineproject.Tests
{
    public class OrderServiceTests
    {
        // Test 1: Calculate order total
        [Fact]
        public void GetTotal_ShouldReturnSubtotalPlusDeliveryCost()
        {
            // Arrange
            var order = new Order
            {
                Subtotal = 600m,
                DeliveryCost = 10m
            };

            // Act
            var result = order.GetTotal();

            // Assert
            Assert.Equal(610m, result);
        }

        // Test 2: Pending -> Shipped
        [Fact]
        public async Task UpdateOrderStatus_PendingToShipped_ShouldSucceed()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Oninecontext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new Oninecontext(options);

            var order = new Order
            {
                BuyerEmail = "test@example.com",
                orderStatus = OrderStatus.Pending,
                Subtotal = 600m,
                DeliveryCost = 10m
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var basketRepositoryMock = new Mock<IBasketRepository>();
            var mapperMock = new Mock<IMapper>();

            var orderService = new OrderServices(
                basketRepositoryMock.Object,
                context,
                mapperMock.Object
            );

            // Act
            var result = await orderService.UpdataOrderStatusAsync(
                order.Id,
                OrderStatus.Shipped
            );

            // Assert
            Assert.True(result);

            context.ChangeTracker.Clear();

            var savedOrder = await context.Orders
                .FirstAsync(o => o.Id == order.Id);

            Assert.Equal(
                OrderStatus.Shipped,
                savedOrder.orderStatus
            );
        }

        // Test 3: Shipped -> Delivered
        [Fact]
        public async Task UpdateOrderStatus_ShippedToDelivered_ShouldSucceed()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Oninecontext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new Oninecontext(options);

            var order = new Order
            {
                BuyerEmail = "test@example.com",
                orderStatus = OrderStatus.Shipped,
                Subtotal = 600m,
                DeliveryCost = 10m
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var basketRepositoryMock = new Mock<IBasketRepository>();
            var mapperMock = new Mock<IMapper>();

            var orderService = new OrderServices(
                basketRepositoryMock.Object,
                context,
                mapperMock.Object
            );

            // Act
            var result = await orderService.UpdataOrderStatusAsync(
                order.Id,
                OrderStatus.Delivered
            );

            // Assert
            Assert.True(result);

            context.ChangeTracker.Clear();

            var savedOrder = await context.Orders
                .FirstAsync(o => o.Id == order.Id);

            Assert.Equal(
                OrderStatus.Delivered,
                savedOrder.orderStatus
            );
        }

        // Test 4: Delivered -> Pending (Not Allowed)
        [Fact]
        public async Task UpdateOrderStatus_DeliveredToPending_ShouldFail()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Oninecontext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new Oninecontext(options);

            var order = new Order
            {
                BuyerEmail = "test@example.com",
                orderStatus = OrderStatus.Delivered,
                Subtotal = 600m,
                DeliveryCost = 10m
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var basketRepositoryMock = new Mock<IBasketRepository>();
            var mapperMock = new Mock<IMapper>();

            var orderService = new OrderServices(
                basketRepositoryMock.Object,
                context,
                mapperMock.Object
            );

            // Act
            var result = await orderService.UpdataOrderStatusAsync(
                order.Id,
                OrderStatus.Pending
            );

            // Assert
            Assert.False(result);

            context.ChangeTracker.Clear();

            var savedOrder = await context.Orders
                .FirstAsync(o => o.Id == order.Id);

            Assert.Equal(
                OrderStatus.Delivered,
                savedOrder.orderStatus
            );
        }
    }
}
