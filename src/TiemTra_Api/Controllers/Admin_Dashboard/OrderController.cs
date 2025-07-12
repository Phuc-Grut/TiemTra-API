using Application.DTOs.Order;
using Application.Interface;
using Domain.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("get-paging-orders")]

        public async Task<IActionResult> GetPagingOrder([FromQuery] OrderFillterDto fillterDto , [FromQuery]  int pageNumber = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await _orderServices.GetPagingOrder(fillterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
