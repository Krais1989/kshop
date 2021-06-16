using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KShop.Carts.Persistence
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CartsCollection { get; set; }
    }

    public class MongoCartRepository : ICartKVRepository
    {
        private readonly IMongoCollection<Cart> _cartStorage;

        public MongoCartRepository(IOptions<MongoSettings> storageSettings)
        {
            var client = new MongoClient(storageSettings.Value.ConnectionString);
            var db = client.GetDatabase(storageSettings.Value.DatabaseName, 
                new MongoDatabaseSettings { });
            _cartStorage = db.GetCollection<Cart>(storageSettings.Value.CartsCollection, 
                new MongoCollectionSettings { 
                });
        }


        public async ValueTask<List<Cart>> GetAllAsync(Func<Cart, bool> where = null, CancellationToken cancellationToken = default)
        {
            var result = await _cartStorage.Find(b => true, 
                new FindOptions
                {

                })
                .ToListAsync(cancellationToken);

            return result;
        }

        public async ValueTask<Cart> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _cartStorage.Find(b => b.ID == id, 
                new FindOptions
                {

                }).FirstOrDefaultAsync(cancellationToken);

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

        public async ValueTask<Cart> InsertAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _cartStorage.InsertOneAsync(cart, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken);
            return cart;
        }

        public async Task ReplaceAsync(string id, Cart cart, CancellationToken cancellationToken = default)
        {
            await _cartStorage.ReplaceOneAsync(b => b.ID == id, cart, 
                new ReplaceOptions
                {
                    IsUpsert = true,
                    BypassDocumentValidation = false,
                }, cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _cartStorage.DeleteOneAsync(b => b.ID == id, 
                new DeleteOptions
                {

                }, cancellationToken);
        }
    }
}
