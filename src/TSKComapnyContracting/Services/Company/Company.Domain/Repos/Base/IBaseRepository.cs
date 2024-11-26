
using System.Linq.Expressions;

namespace Company.Domain.Repos.Base;

public interface IBaseRepository<T> where T : class
{
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, bool asNoTracking = false);

    T GetFirstOrDefaultIncluding(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);

    T GetLastOrDefault(Expression<Func<T, bool>> filter, Expression<Func<T, object>> filterOrder);

    IEnumerable<T> GetAll();

    Task< IEnumerable<T>> GetQuery(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
    IEnumerable<T> GetQuery(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);

    void Add(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    void AddRange(IEnumerable<T> entities);

    void Update(T entity);

    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

    ICollection<TType> GetSelectedQuery<TType>(Expression<Func<T, bool>> where, Expression<Func<T, TType>> select, params Expression<Func<T, object>>[] includes);

    IEnumerable<T> GetQueryValidation(Expression<Func<T, bool>> filter);

    void Save();

    T? GetById(object id);

    T? GetByIdAsNoTracking(object id);

    TKey GetEntityIdByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TKey>> keySelector) where TEntity : class;

    bool Any(Expression<Func<T, bool>> predicate);


    List<TKey> GetEntityIdsByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TKey>> keySelector) where TEntity : class;

    public List<TKey> GetEntityIdsByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TKey>> keySelector, params Expression<Func<TEntity, object>>[] columnSelectors) where TEntity : class;
    void UpdateRange(IEnumerable<T> entityList);
    void ExecuteSqlCommand(string sqlQuery);
    ICollection<TType> GetSelectedQueryAsNoTracking<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select, params Expression<Func<T, object>>[] includes) where TType : class;
    IEnumerable<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector, Expression<Func<T, bool>> predicate = null, bool asNoTracking = false);

    T? GetByIdAsNoTracking(object id, List<string>? includeProperties);
    IEnumerable<T> GetQuery(Expression<Func<T, bool>> filter, IEnumerable<string> excludeProperties, params Expression<Func<T, object>>[] includes);
    TResult GetMaxColumn<TResult>(Expression<Func<T, TResult>> selector);
    TResult GetMinColumn<TResult>(Expression<Func<T, TResult>> selector);
}
