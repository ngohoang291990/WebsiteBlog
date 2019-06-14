using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.DataManagement.MongoDB
{
    [BsonIgnoreExtraElements]
    public abstract class MongoModelBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ObjectType { get; set; }

        public bool IsDeleted { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LastModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
