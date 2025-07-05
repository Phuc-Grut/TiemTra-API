using Domain.Data.Entities;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;       
        }
        public async Task AddCart(Cart cart)
        {
            await _context.Cart.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddCartItemAsync(Cart cart, CartItem newItem, CancellationToken cancellationToken)
        {
            cart.CartItem.Add(newItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Cart?> GetCartByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Cart
                .Include(c => c.CartItem)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductVariations)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpdateCartItemAsync(Cart cart, CartItem updatedItem, CancellationToken cancellationToken)
        {
            _context.CartItems.Update(updatedItem);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            _context.Cart.Update(cart);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            _context.CartItems.Remove(cartItem);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
