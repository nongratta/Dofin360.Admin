using MongoDB.Bson;
using System.Linq.Expressions;
namespace Dofin360.Admin.Core;
public interface IRepository<TItem>
{
    Task<TItem> GetById(ObjectId id);
    Task<TItem> GetById(Guid id);
    Task<TItem> Add(object item);
    Task<List<TItem>> Add(List<TItem> items);
    Task<TItem> Update(TItem item, ObjectId id);
    Task<TItem> Update(TItem item, Guid id);
    Task Delete(Guid id);
    Task Delete(ObjectId id);
    Task Delete(Expression<Func<TItem, bool>> filter);
    Task<List<TItem>> Find(Expression<Func<TItem, bool>> predicate);
    Task<List<TItem>> Find(Expression<Func<TItem, bool>> predicate, int skip, int take);
    Task<List<TItem>> Find<TResult>(Expression<Func<TItem, bool>> predicate, Expression<Func<TItem, object>> orderBy, int skip, int take);
    Task<List<TItem>> Find<TResult>(Expression<Func<TItem, bool>> predicate, Expression<Func<TItem, object>> orderBy, Expression<Func<TItem, TItem>> projection, int skip, int take);
    Task<long> Count(Expression<Func<TItem, bool>> predicate);
    Task<long> Count();
}
