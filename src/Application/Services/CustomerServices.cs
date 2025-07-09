using Application.DTOs.Order;
using Application.Interface;
using Domain.Data.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerServices : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private async Task<string> GenerateUniqueCustomerCodeAsync()
        {
            var random = new Random();
            string orderCode;
            bool exists;
            do
            {
                int ranDomNumber = random.Next(0, 9999);
                orderCode = $"KH{ranDomNumber:D3}";
                exists = await _customerRepository.CustomerCodeExistsAsync(orderCode);
            }
            while (exists);
            return orderCode;
        }

        public async Task<Guid> GetOrCreateCustomerAsync(CreateOrderRequest request, Guid? userId, CancellationToken cancellationToken)
        {
            if (userId.HasValue)
            {
                var customer = await _customerRepository.FindByCustomerIdAsync(userId.Value, cancellationToken);
                if (customer != null)
                {
                    return customer.CustomerId;
                }
                else
                {
                    var user = await _userRepository.GetUserByIdAsync(userId.Value, cancellationToken);

                    var newCustomer = new Customer
                    {
                        CustomerId = userId.Value,
                        CustomerCode = await GenerateUniqueCustomerCodeAsync(),
                        CustomerName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        UserId = userId.Value,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId.Value
                    };
                    await _customerRepository.CreateCustomerAsync(newCustomer, cancellationToken);
                    return newCustomer.CustomerId;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.RecipientPhone)) 
                    throw new ArgumentException("Vui lòng nhập số điện thoại nhận hàng");

                var existingCustomer = await _customerRepository.FindByPhoneNumberAsync(request.RecipientPhone, cancellationToken);

                if(existingCustomer != null)
                    return existingCustomer.CustomerId;

                var newCustomer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    CustomerCode = await GenerateUniqueCustomerCodeAsync(),
                    CustomerName = request.RecipientName,
                    PhoneNumber = request.RecipientPhone,
                    Address = request.RecipientAddress,
                    CreatedAt = DateTime.UtcNow
                };

                await _customerRepository.CreateCustomerAsync(newCustomer, cancellationToken);
                return newCustomer.CustomerId;
            }
        }
    }
}
