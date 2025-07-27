using Application.Interface;
using Application.Services;
using Domain.DTOs.Customer;
using Domain.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiemTra_Api.Controllers.Admin_Dashboard
{
    [Route("api/admin/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
                _customerService = customerService;
        }

        [HttpGet("get-paging-customer")]
        public async Task<IActionResult> GetPagingOrder([FromQuery] CustomerFilterDto fillterDto, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await _customerService.GetPagingAsync(fillterDto, pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
