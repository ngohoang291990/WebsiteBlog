using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Blog.DataManagement.MongoDB
{
    public interface IMongoRepository<T> where T : MongoModelBase
    {
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken));

        Task AddAsync(T item, CancellationToken cancellationToken = default(CancellationToken));

        Task AddManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(T item, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(
          string id,
          UpdateDefinition<T> update,
          CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(
          FilterDefinition<T> filter,
          UpdateDefinition<T> update,
          CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateManyAsync(IEnumerable<T> items, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(string id, CancellationToken cancellationToken = default(CancellationToken), bool softDelete = true);

        Task DeleteManyAsync(IEnumerable<string> ids, bool softDelete, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<T>> FindAsync(
          FilterDefinition<T> filter,
          CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<P>> FindAsync<P>(
          FilterDefinition<T> filter,
          Expression<Func<T, P>> projection,
          CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> CheckExistence(string field, object value, CancellationToken token = default(CancellationToken));

        Task Seed(Func<IEnumerable<T>> seedFactory);

        void SetObjectType(string objectType);

        FilterDefinitionBuilder<T> Filter { get; }

        UpdateDefinitionBuilder<T> Update { get; }

        FilterDefinition<T> TypeCheckFilter();

        FilterDefinition<T> FilterBase();

        SortDefinition<T> CreateSortDefinition(
          IEnumerable<KeyValuePair<string, bool>> order);

        void PatchProperty(string filterPath, string updatePath, string id, object payload);
    }
}