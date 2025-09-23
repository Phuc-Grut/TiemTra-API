using Application.DTOs.Order;
using Domain.DTOs;
using Domain.DTOs.Customer;

namespace Application.Interface
{
    public interface ICustomerService
    {
        Task<Guid> GetOrCreateCustomerAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken);

        Task<PagedResult<CustomerDto>> GetPagingAsync(CustomerFilterDto filterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}