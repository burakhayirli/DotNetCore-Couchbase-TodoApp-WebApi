using Core.Entities;
using Couchbase.Core;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public abstract class CouchbaseRepository<T>
        where T : CouchbaseEntity<T>
    {
        protected readonly IBucket _bucket;

        protected CouchbaseRepository(ITodoBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket();
        }

        protected T Get(Guid id)
        {
            var result = _bucket.Get<T>(CreateKey(id));
            if (!result.Success)
                throw result.Exception;
            return result.Value;
        }

        protected IEnumerable<T> GetAll(int limit = 10)
        {
            var query = new QueryRequest(
                $@"SELECT t.* 
                FROM {_bucket.Name} as t 
                WHERE type = '{Type}' 
                LIMIT {limit};");

            var result = _bucket.Query<T>(query);
            if (!result.Success)
                throw result.Exception;

            return result.Rows;
        }

        protected T Create(T item)
        {
            var result = _bucket.Insert(CreateKey(item.Id), item);
            if (!result.Success)
                throw result.Exception;

            return result.Value;
        }

        protected T Update(T item)
        {
            var result = _bucket.Replace(CreateKey(item.Id), item);
            if (!result.Success)
                throw result.Exception;

            return result.Value;
        }

        protected void Delete(Guid id)
        {
            var result = _bucket.Remove(CreateKey(id));
            if (!result.Success)
                throw result.Exception;
        }

        protected string CreateKey(Guid id) => $"{Type}::{id}";
        protected string Type => typeof(T).Name.ToLower();
    }
}
