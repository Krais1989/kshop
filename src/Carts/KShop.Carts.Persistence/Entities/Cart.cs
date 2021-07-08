using KShop.Shared.Domain.Contracts;
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
        public bool Checked { get; set; }
        public string Title { get; set; }
        public Money Price { get; set; }
        public string Descriptions { get; set; }
        public string Image { get; set; }

        public CartPosition() { }

        public CartPosition(
            uint productID,
            uint quantity,
            bool @checked,
            string title,
            Money price,
            string descriptions,
            string image)
        {
            ProductID = productID;
            Quantity = quantity;
            Checked = @checked;
            Title = title;
            Price = price;
            Descriptions = descriptions;
            Image = image;
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
                exist.Checked = pos.Checked;
            }
        }

        public void MergeRange(IEnumerable<CartPosition> positions)
        {
            foreach (var pos in positions)
            {
                SetPosition(pos);
            }
        }

        public void SetRange(IEnumerable<CartPosition> positions)
        {
            Clear();
            MergeRange(positions);
        }

        public void RemovePosition(uint productId)
        {
            Positions.RemoveAll(e => e.ProductID == productId);
        }

        public void RemoveRange(IEnumerable<uint> ProductsIDs)
        {
            foreach (var id in ProductsIDs)
            {
                RemovePosition(id);
            }
        }

        public void Clear()
        {
            Positions = new List<CartPosition>();
        }
    }
}
