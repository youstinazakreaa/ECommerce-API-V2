using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onine.core.Entites;
using onine.Repository.Data;
using onineproject.DTOs;
using Microsoft.EntityFrameworkCore;

namespace onineproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly Oninecontext _Oninecontext;

        public DeliveryMethodController(Oninecontext oninecontext)
        {
            _Oninecontext= oninecontext;

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _Oninecontext.DeliveryMethods.AsNoTracking()
                .Select(d=>new DeliveryMethodDto
                {
                    Id = d.Id,
                    ShortName = d.ShortName,
                    Description = d.Description,
                    DeliveryTime = d.DeliveryTime,
                    Price = d.Price
                })
                .ToListAsync();
            return Ok(deliveryMethods);
        }

    }
}
