using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICustomerRepository
    {
        Task<Customer> FindByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);

        Task<Customer> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task CreateCustomerAsync(Customer customer, CancellationToken cancellationToken);

        Task<bool> CustomerCodeExistsAsync(string customerCode);
        Task<PagedResult<CustomerDto>> GetPagingAsync(CustomerFilterDto filterDto, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
