using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Core.Entities
{
    public class Cart : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Items { get; set; }

        [BsonIgnore]
        public List<Item> ItemList { get; set; }
    }
}
