using onine.core.Entites;
using onine.core.DTOs;
using onine.core.Enums;

namespace onine.services.Interfaces
{
    public interface IOrderServices
    {
        Task<Order?> CreateOrderAsync(  string buyerEmail, CreateOrderDto orderDto);

        Task<Order?> GetOrderByIdAsync(
            int orderId,
            string buyerEmail);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(
            string buyerEmail);
        Task <bool> UpdataOrderStatusAsync (int orderId,  OrderStatus newstatus);
        Task <bool> CancelOrderAsync(int orderId, string buyerEmail);
    }
}