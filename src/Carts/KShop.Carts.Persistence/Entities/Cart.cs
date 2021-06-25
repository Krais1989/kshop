using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KShop.Carts.Persistence
{
    public class CartPosition
    {
        public uint ProductID { get; set; }
        public uint Quantity { get; set; }
        public CartPosition() { }
        public CartPosition(uint productID, uint quantity)
        {
            ProductID = productID;
            Quantity = quantity;
        }
    }

    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string ID { get; set; }

//      [BsonDictionaryOptions(DictionaryRepresentation.)]
        public List<CartPosition> Positions { get; set; } = new List<CartPosition>();

        public CartPosition Get(uint productId) => Positions.FirstOrDefault(e => e.ProductID == productId);

        public void SetPosition(CartPosition pos)
        {
            var exist = Get(pos.ProductID);
            if (exist == null)
            {
                Positions.Add(pos);
            }
            else
            {
                exist.Quantity = pos.Quantity;
            }
        }

        public void SetRange(IEnumerable<CartPosition> positions)
        {
            foreach (var pos in positions)
            {
                SetPosition(pos);
            }
        }

        public void RemovePosition(uint productId)
        {
            Positions.RemoveAll(e => e.ProductID == productId);
        }

        public void RemoveRange(IEnumerable<uint> productIds)
        {
            foreach (var id in productIds)
            {
                RemovePosition(id);
            }
        }
    }
}
