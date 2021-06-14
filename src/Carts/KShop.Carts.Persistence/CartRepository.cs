using KShop.Carts.Persistence.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Persistence
{
    public class CartsStorageSettings
    {
        public MongoClientSettings ClientSettings { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CartsCollection { get; set; }
    }

    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _cartStorage;

        public CartRepository(IOptions<CartsStorageSettings> storageSettings)
        {
            var client = new MongoClient(storageSettings.Value.ClientSettings);
            var db = client.GetDatabase(storageSettings.Value.DatabaseName);
            _cartStorage = db.GetCollection<Cart>(storageSettings.Value.CartsCollection);
        }


        public async ValueTask<List<Cart>> GetAllAsync(Func<Cart, bool> where = null, CancellationToken cancellationToken = default)
        {
            var result = await _cartStorage
                .Find(b => where == null ? true : where(b), _findAllOptions)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async ValueTask<Cart> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _cartStorage.Find(b => b.ID == id, _findOptions).FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                result = new Cart
                {
                    ID = id,
                    Positions = new CartPositions()
                };
                await _cartStorage.InsertOneAsync(result, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
            };
            return result;
        }

        public async ValueTask<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _cartStorage.InsertOneAsync(cart, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
            return cart;
        }

        public async Task UpdateAsync(string id, Cart cart, CancellationToken cancellationToken = default)
            => await _cartStorage.ReplaceOneAsync(b => b.ID == id, cart, _replaceOptions, cancellationToken);

        public async Task RemoveAsync(string id, CancellationToken cancellationToken = default)
            => await _cartStorage.DeleteOneAsync(b => b.ID == id, _deleteOptions, cancellationToken);

        #region Options
        private readonly InsertOneOptions _insertOptions =
            new InsertOneOptions
            {
                BypassDocumentValidation = false
            };

        private readonly ReplaceOptions _replaceOptions =
            new ReplaceOptions
            {
                IsUpsert = true,
                BypassDocumentValidation = false,
            };

        private readonly DeleteOptions _deleteOptions =
            new DeleteOptions
            {

            };

        private readonly FindOptions _findOptions =
            new FindOptions
            {

            };

        private readonly FindOptions _findAllOptions =
            new FindOptions
            {

            };
        #endregion
    }
}
