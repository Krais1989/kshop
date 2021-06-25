using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Persistence
{
    /// <summary>
    /// Cart Key/Value Repository
    /// </summary>
    public interface ICartKVRepository
    {
        ValueTask<Cart> InsertAsync(Cart cart, CancellationToken cancellationToken = default);
        ValueTask<List<Cart>> GetAllAsync(Func<Cart, bool> where = null, CancellationToken cancellationToken = default);
        ValueTask<Cart> GetAsync(string id, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task ReplaceAsync(string id, Cart cart, CancellationToken cancellationToken = default);
    }
}