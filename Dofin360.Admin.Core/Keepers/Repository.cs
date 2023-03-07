
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Dofin360.Admin.Core;

public class Repository<TItem> : IRepository<TItem>
{
    protected readonly IMongoDatabase Database;
    protected readonly string CollectionName;
    protected readonly string ConnectionString;
    protected IMongoCollection<TItem> Collection => Database.GetCollection<TItem>(CollectionName);
    protected Repository(string connectionString, string collectionName)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(connectionString);
        Database = new MongoClient(connectionString).GetDatabase(mongoUrlBuilder.DatabaseName);
        CollectionName = collectionName;
        ConnectionString = connectionString;
    }
    public async Task<TItem> GetById(ObjectId id)
    {
        return await Collection.Find(new BsonDocument("_id", id)).FirstOrDefaultAsync();
    }
    public async Task<TItem> GetById(Guid id)
    {
        var binData = new BsonBinaryData(id, GuidRepresentation.Standard);
        return await Collection.Find(new BsonDocument("_id", binData)).FirstOrDefaultAsync();
    }
    public async Task<TItem> Add(object item)
    {
        TItem x = (TItem)item;
        await Collection.InsertOneAsync(x);
        return x;
    }

    public async Task<List<TItem>> Add(List<TItem> items)
    {
        await Collection.InsertManyAsync(items);
        return items;
    }
    public async Task<TItem> Update(TItem item, ObjectId id)
    {
        try
        {
            await Collection.ReplaceOneAsync(new BsonDocument("_id", id), item, new ReplaceOptions { IsUpsert = true });
        }
        catch (Exception)
        {
            await Collection.InsertOneAsync(item);
        }
        return item;
    }
    public async Task<TItem> Update(TItem item, Guid id)
    {
        try
        {
            var binData = new BsonBinaryData(id, GuidRepresentation.Standard);
            await Collection.ReplaceOneAsync(new BsonDocument("_id", binData), item, new ReplaceOptions { IsUpsert = true });
        }
        catch (Exception)
        {
            await Collection.InsertOneAsync(item);
        }
        return item;
    }
    public async Task Delete(Guid id)
    {
        await Collection.DeleteOneAsync(new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard)));
    }
    public async Task Delete(ObjectId id)
    {
        await Collection.DeleteOneAsync(new BsonDocument("_id", id));
    }
    public async Task Delete(Expression<Func<TItem, bool>> filter)
    {
        await Collection.DeleteManyAsync(filter);
    }
    public async Task<List<TItem>> Find(Expression<Func<TItem, bool>> predicate)
    {
        return await Collection.FindAsync(predicate).Result.ToListAsync();
    }

    public async Task<List<TItem>> Find(Expression<Func<TItem, bool>> predicate, int skip, int take)
    {
        var options = take != 0 ? new FindOptions<TItem>() { Skip = skip, Limit = take } : new FindOptions<TItem> { Skip = skip };
        var result = await Collection.FindAsync(predicate, options);
        return await result.ToListAsync();
    }

    public async Task<List<TItem>> Find<TResult>(Expression<Func<TItem, bool>> predicate, Expression<Func<TItem, object>> orderBy, int skip, int take)
    {

        var sort = Builders<TItem>.Sort.Ascending(orderBy);
        var options = take != 0
            ? new FindOptions<TItem>() { Skip = skip, Limit = take, Sort = sort }
            : new FindOptions<TItem>() { Skip = skip, Sort = sort };
        var result = await Collection.FindAsync(predicate, options);
        return await result.ToListAsync();
    }

    public async Task<List<TItem>> Find<TResult>(Expression<Func<TItem, bool>> predicate, Expression<Func<TItem, object>> orderBy, Expression<Func<TItem, TItem>> projection, int skip, int take)
    {
        var proj = Builders<TItem>.Projection.Expression(projection);
        var sort = Builders<TItem>.Sort.Ascending(orderBy);
        var options = take != 0
            ? new FindOptions<TItem>() { Skip = skip, Limit = take, Sort = sort, Projection = proj }
            : new FindOptions<TItem>() { Skip = skip, Sort = sort, Projection = proj };
        var result = await Collection.FindAsync(predicate, options);
        return await result.ToListAsync();
    }

    public async Task<long> Count(Expression<Func<TItem, bool>> predicate) => await Collection.CountDocumentsAsync(predicate);
    [BsonIgnoreExtraElements]
    class CountResult
    {
        [BsonElement("count")]
        public long Count { get; set; }
    }
    public async Task<long> Count()
    {
        var str = "{collstats: '" + $"{CollectionName}" + "'}";
        var result = await Database.RunCommandAsync<CountResult>(str);
        return result.Count;
    }
}
