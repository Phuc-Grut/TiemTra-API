using Domain.Data.Entities;

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

        Task DeleteByIdAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}