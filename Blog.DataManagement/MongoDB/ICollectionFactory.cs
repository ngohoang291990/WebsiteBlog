using MongoDB.Driver;

namespace Blog.DataManagement.MongoDB
{
    public interface ICollectionFactory : ISingletonDependency, IDependency
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);

        IMongoDatabase Database { get; }
    }
}