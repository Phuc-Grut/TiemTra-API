using Application.Interface;
using Domain.Data.Entities;
using Domain.DTOs;
using Domain.DTOs.Customer;
using Domain.Enum;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _dbContext.Customer.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CustomerCodeExistsAsync(string customerCode)
        {
            return await _dbContext.Customer.AsNoTracking().AnyAsync(c => c.CustomerCode == customerCode);
        }

        public async Task<Customer> FindByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _dbContext.Customer.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        }

        public async Task<Customer> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return await _dbContext.Customer.AsNoTracking().FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
        }

        public async Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public async Task<PagedResult<CustomerDto>> GetPagingAsync(CustomerFilterDto filterDto, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Customer.Include(c => c.Orders).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDto.Keyword))
            {
                query = query.Where(c =>
                    c.CustomerName.Contains(filterDto.Keyword) ||
                    c.CustomerCode.Contains(filterDto.Keyword) ||
                    c.PhoneNumber.Contains(filterDto.Keyword));
            }

            if (!string.IsNullOrWhiteSpace(filterDto.CustomerCode))
            {
                query = query.Where(c => c.CustomerCode.Contains(filterDto.CustomerCode));
            }

            if (!string.IsNullOrWhiteSpace(filterDto.PhoneNumber))
            {
                query = query.Where(c => c.PhoneNumber.Contains(filterDto.PhoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(filterDto.SortBy))
            {
                switch (filterDto.SortBy)
                {
                    case "name_asc":
                        query = query.OrderBy(c => c.CustomerName);
                        break;

                    case "name_desc":
                        query = query.OrderByDescending(c => c.CustomerName);
                        break;

                    default:
                        query = query.OrderByDescending(c => c.CustomerCode);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(c => c.CustomerCode);
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync(cancellationToken);

            var customerDtos = items.Select(c => new CustomerDto
            {
                Avatar = c.AvatarUrl,
                CustomerId = c.CustomerId,
                CustomerCode = c.CustomerCode,
                CustomerName = c.CustomerName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                OrderSuccessful = c.Orders?.Count(o => o.OrderStatus == OrderStatus.Delivered) ?? 0
            }).ToList();

            if (filterDto.SortBy == "order_success_desc")
            {
                customerDtos = customerDtos.OrderByDescending(c => c.OrderSuccessful).ToList();
            }
            else if (filterDto.SortBy == "order_success_asc")
            {
                customerDtos = customerDtos.OrderBy(c => c.OrderSuccessful).ToList();
            }

            return new PagedResult<CustomerDto>
            {
                Items = customerDtos,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };
        }
    }
}