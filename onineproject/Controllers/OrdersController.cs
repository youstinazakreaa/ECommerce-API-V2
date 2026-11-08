
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onine.core.Entites;
using onine.services.Interfaces;
using onineproject.DTOs;
using System.Security.Claims;
using onine.core.DTOs;
using onine.core.Enums;

namespace onineproject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(
            [FromBody] CreateOrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var order = await _orderServices.CreateOrderAsync(email, orderDto);

            if (order == null)
            {
                return BadRequest("Problem creating order");
            }

            return Ok(MapToDto(order));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]   
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var orders = await _orderServices.GetOrdersForUserAsync(email);

            var orderDtos = orders.Select(MapToDto).ToList();

            return Ok(orderDtos);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var order = await _orderServices.GetOrderByIdAsync(id, email);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(MapToDto(order));
        }


        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                Status = order.orderStatus.ToString(),
                SubTotal = order.Subtotal,
                Deliverycost = order.DeliveryCost,

                OrderItems = order.orderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.productItemOrdered.ProductId,
                    ProductName = item.productItemOrdered.ProductName,
                    PictureUrl = item.productItemOrdered.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }




        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            if (!updateStatusDto.newstatus.HasValue || !Enum.IsDefined(typeof(OrderStatus), updateStatusDto.newstatus.Value))
            {
                return BadRequest("New status is required.");
            }
            var result = await _orderServices.UpdataOrderStatusAsync(orderId, updateStatusDto.newstatus.Value);

            if (!result)
            {
                return BadRequest("Order not found or Status Change is not Allowed");
            }
            return Ok("Order Status Updated Sucessfully ");
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var result = await _orderServices.CancelOrderAsync(orderId, email);
            if (!result)
            {
                return BadRequest("Order not found or cannot be canceled.");
            }
            return Ok("Order canceled successfully.");
        }

    }
}
