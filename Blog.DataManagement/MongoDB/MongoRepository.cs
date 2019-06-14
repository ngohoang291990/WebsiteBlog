using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Blog.DataManagement.MongoDB
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoModelBase
    {
        private IMongoCollection<T> _collection;
        private string _objectType;
        protected ICollectionFactory CollectionFactory { get; }
        public MongoRepository(ICollectionFactory collectionFactory)
        {
            CollectionFactory = collectionFactory;
        }

        protected IMongoCollection<T> Collection
        {
            get
            {
                if (this._collection != null)
                    return this._collection;
                this._collection = this.CollectionFactory.GetCollection<T>(this.GetObjectType());
                return this._collection;
            }
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.Collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            //return await (Task<T>)IFindFluentExtensions.FirstOrDefaultAsync<T, T>((IFindFluent<TDocument,TProjection>), cancellationToken);
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            item.ObjectType = this.GetObjectType();
            item.CreatedDate=new DateTime?(DateTime.Now);
            item.LastModifiedDate=new DateTime?(DateTime.Now);
            await this.Collection.InsertOneAsync(item, (InsertOneOptions) null, cancellationToken);
        }

        public async Task AddManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default(CancellationToken))
        {
            string objectType = this.GetObjectType();
            T[] objArray = items as T[] ?? items.ToArray<T>();
            foreach (T item in objArray)
            {
                item.ObjectType = objectType;
                item.CreatedDate = new DateTime?(DateTime.Now);
                item.LastModifiedDate = new DateTime?(DateTime.Now);
            }

            await this.Collection.InsertManyAsync((IEnumerable<T>) objArray, (InsertManyOptions) null,
                cancellationToken);
        }

        public async Task UpdateAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            item.ObjectType = this.GetObjectType();
            item.LastModifiedDate = new DateTime?(DateTime.Now);
            await Collection.ReplaceOneAsync(x => x.Id.Equals(item.Id), item,
                new UpdateOptions { IsUpsert = true });
        }

        public Task UpdateAsync(string id, UpdateDefinition<T> update, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task UpdateManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken), bool softDelete = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(IEnumerable<string> ids, bool softDelete, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<P>> FindAsync<P>(FilterDefinition<T> filter, Expression<Func<T, P>> projection,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckExistence(string field, object value, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Seed(Func<IEnumerable<T>> seedFactory)
        {
            throw new NotImplementedException();
        }

        public void SetObjectType(string objectType)
        {
            throw new NotImplementedException();
        }

        public FilterDefinitionBuilder<T> Filter { get; }
        public UpdateDefinitionBuilder<T> Update { get; }
        public FilterDefinition<T> TypeCheckFilter()
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<T> FilterBase()
        {
            throw new NotImplementedException();
        }

        public SortDefinition<T> CreateSortDefinition(IEnumerable<KeyValuePair<string, bool>> order)
        {
            throw new NotImplementedException();
        }

        public void PatchProperty(string filterPath, string updatePath, string id, object payload)
        {
            throw new NotImplementedException();
        }


        protected string GetObjectType()
        {
            if (!string.IsNullOrEmpty(this._objectType))
                return this._objectType;
            MongoCollectionAttribute customAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>();
            this._objectType = string.IsNullOrEmpty(customAttribute?.ObjectType) ? typeof(T).FullName?.Replace(".", "_") : customAttribute?.ObjectType;
            return this._objectType;
        }
    }
}