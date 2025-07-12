using Application.Interface;
using Domain.Data.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
