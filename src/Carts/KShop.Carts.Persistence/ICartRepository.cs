using KShop.Carts.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Persistence
{
    public interface ICartRepository
    {
        ValueTask<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
        ValueTask<List<Cart>> GetAllAsync(Func<Cart, bool> where = null, CancellationToken cancellationToken = default);
        ValueTask<Cart> GetAsync(string id, CancellationToken cancellationToken = default);
        Task RemoveAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(string id, Cart cart, CancellationToken cancellationToken = default);
    }
}