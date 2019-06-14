namespace Blog.DataManagement.MongoDB
{
    public class GenericModel<T> : MongoModelBase where T : class
    {
        public T Data { get; set; }
    }
}
