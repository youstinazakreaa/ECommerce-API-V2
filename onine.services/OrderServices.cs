
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using onine.core.DTOs;
using onine.core.Entites;
using onine.core.Enums;
using onine.core.Repositories;
using onine.Repository.Data;
using onine.services.Interfaces;

namespace onine.services
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly Oninecontext _context;
        private readonly IMapper _mapper;

        public OrderServices(
            IBasketRepository basketRepository,
            Oninecontext context,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Order?> CreateOrderAsync(
            string buyerEmail,
            CreateOrderDto orderDto)
        {
            // 1. Get basket
            if (orderDto == null || orderDto.Address == null)
                return null;

            var basket = await _basketRepository
                .GetBasketAsync(orderDto.BasketId);

            if (basket?.Items == null || !basket.Items.Any())
                return null;

            // 2. Create order items
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await _context.Set<Product>()
                    .FirstOrDefaultAsync(p => p.Id == item.Id);

                if (product == null || item.Quantity <= 0)
                    return null;

                var productItemOrdered = new ProductItemOrdered
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    productItemOrdered = productItemOrdered,
                    Price = product.Price,
                    Quantity = item.Quantity
                };

                orderItems.Add(orderItem);
            }

            // 3. Calculate subtotal
            var subtotal = orderItems.Sum(
                item => item.Price * item.Quantity);

            // 4. Get delivery method
            var deliveryMethod = await _context.Set<DeliveryMethod>()
                .FirstOrDefaultAsync(
                    dm => dm.Id == orderDto.DeliveryMethodId);

            if (deliveryMethod == null)
                return null;

            var deliveryPrice = deliveryMethod.Price;

            // 5. Create shipping address
            var shippingAddress = new OrderAddress
            {
                FirstName = orderDto.Address.FirstName,
                LastName = orderDto.Address.LastName,
                Street = orderDto.Address.Street,
                City = orderDto.Address.City,
                Country = orderDto.Address.Country
            };

            // 6. Create order
            var order = new Order
            {
                BuyerEmail = buyerEmail,
                OrderDate = DateTimeOffset.UtcNow,
                orderStatus = OrderStatus.Pending,
                Subtotal = subtotal,
                orderItems = orderItems,
                ShipToAddress = shippingAddress,
                DeliveryMethodId = deliveryMethod.Id,
                DeliveryCost = deliveryPrice
            };

            // 7. Save order
            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();

            // 8. Return created order
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(
            int orderId,
            string buyerEmail)
        {
            return await _context.Set<Order>()
                .Include(o => o.orderItems)
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.BuyerEmail == buyerEmail);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(
            string buyerEmail)
        {
            return await _context.Set<Order>()
                .Include(o => o.orderItems)
                .Where(o => o.BuyerEmail == buyerEmail)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> UpdataOrderStatusAsync(
            int orderId,
            OrderStatus newstatus)
        {
            if (orderId <= 0)
            {
                return false;
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            if (order.orderStatus == newstatus)
            {
                return true;
            }

            bool isValidTransition =
                (order.orderStatus == OrderStatus.Pending &&
                 newstatus == OrderStatus.Shipped) ||

                (order.orderStatus == OrderStatus.Shipped &&
                 newstatus == OrderStatus.Delivered);

            if (!isValidTransition)
            {
                return false;
            }

            order.orderStatus = newstatus;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelOrderAsync(
            int orderId,
            string buyerEmail)
        {
            if (orderId <= 0 ||
                string.IsNullOrWhiteSpace(buyerEmail))
            {
                return false;
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.BuyerEmail == buyerEmail);

            if (order == null)
            {
                return false;
            }

            if (order.orderStatus == OrderStatus.Cancelled)
            {
                return true;
            }

            if (order.orderStatus != OrderStatus.Pending)
            {
                return false;
            }

            order.orderStatus = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
