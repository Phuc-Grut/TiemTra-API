using Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ICartRepository
    {
        Task AddCart(Cart cart);
        Task<Cart?> GetCartByUserId(Guid userId, CancellationToken cancellationToken);
        Task AddCartItemAsync(Cart cart, CartItem newItem, CancellationToken cancellationToken);
        Task UpdateCartItemAsync(Cart cart, CartItem updatedItem, CancellationToken cancellationToken);

        Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken);

        Task RemoveCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);

    }
}
