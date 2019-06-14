using System;

namespace Blog.DataManagement.MongoDB
{
    public class MongoCollectionAttribute:Attribute
    {
        public string ObjectType { get; set; }
    }
}