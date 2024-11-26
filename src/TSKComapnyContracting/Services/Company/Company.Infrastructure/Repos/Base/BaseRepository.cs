

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Company.Domain.Repos.Base;
using Company.Infrastructure.Context;

namespace Company.Infrastructure.Repos.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    internal DbSet<T> dbSet;
    private readonly AppDbContext _dbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
        this.dbSet = _dbContext.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        dbSet.AddRange(entities);
    }

    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = dbSet;
        return query.ToList();
    }

    public T? GetById(object id)
    {
        Type type = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().GetKeyType();
        //Activator.CreateInstance(type);
        var entityId = Convert.ChangeType(id, type);
        //return _dbContext.Set<T>().Find(entityId);
        return dbSet.Find(entityId);
    }

    public T? GetByIdAsNoTracking(object id)
    {
        Type type = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().GetKeyType();
        var entityId = Convert.ChangeType(id, type);
        var entity = dbSet.Find(entityId);
        _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public T? GetByIdAsNoTracking(object id, List<string> navigationProperties)
    {
        var entityType = _dbContext.Model.FindEntityType(typeof(T));
        var primaryKey = entityType.FindPrimaryKey();
        var keyProperties = primaryKey.Properties.Select(x => x.PropertyInfo);
        var keyProperty = keyProperties.First();
        var idValue = Convert.ChangeType(id, keyProperty.PropertyType);

        var parameter = Expression.Parameter(typeof(T), "e");
        var propertyAccess = Expression.Property(parameter, keyProperty);
        var constantExpression = Expression.Constant(idValue);
        var equalExpression = Expression.Equal(propertyAccess, constantExpression);

        var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

        var query = dbSet.AsQueryable();

        foreach (var property in navigationProperties)
        {
            PropertyInfo navigationProperty = typeof(T).GetProperty(property);
            if (navigationProperty != null && typeof(IEnumerable<object>).IsAssignableFrom(navigationProperty.PropertyType))
            {
                query = query.Include(property);
            }
        }

        var entity = query.FirstOrDefault(lambda);

        _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }



    public string GetReferenceName(T entity, object id, string columnName)
    {
        string refenceName = "";
        Type type = _dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().GetKeyType();
        //Activator.CreateInstance(type);
        var entityId = Convert.ChangeType(id, type);
        var entityEntry = dbSet.Find(entityId);
        Type entityType = entityEntry.GetType();
        var property = entityType.GetProperties().Where(p => p.Name.ToLower() == columnName.ToLower()).FirstOrDefault();
        if (property != null)
        {
            refenceName = property.GetValue(entityEntry).ToString();
        }
        _dbContext.Entry(entity).State = EntityState.Detached;
        return refenceName;
    }

    public T GetFirstOrDefaultIncluding(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);
        if (includes.Length > 0)
        {
            query = query.Include(includes[0]);
        }
        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }
        if (asNoTracking)
        {
            return query.AsNoTracking().FirstOrDefault();
        }

        return query.FirstOrDefault();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter, bool asNoTracking = false)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);
        if (asNoTracking)
        {
            return query.AsNoTracking().FirstOrDefault();
        }
        return query.FirstOrDefault();
    }

    public T GetLastOrDefault(Expression<Func<T, bool>> filter, Expression<Func<T, object>> filterOrder)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter).OrderBy(filterOrder);
        return query.LastOrDefault();
    }

    public async Task<IEnumerable<T>> GetQuery(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes.Length > 0)
        {
            query = query.Include(includes[0]);
        }
        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }
        return await query.ToListAsync();
    }



    public IEnumerable<T> GetQuery(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object>>?[] includes)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes != null && includes.Length > 0)
        {
            query = query.Include(includes[0]);
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }
        }

        if (asNoTracking)
        {
            return query.AsNoTracking().ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public IEnumerable<T> GetQuery(Expression<Func<T, bool>> filter,
                                      IEnumerable<string> excludeProperties,
                                      params Expression<Func<T, object>>[] includes)

    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Define the parameter expression for the lambda
        var parameter = Expression.Parameter(typeof(T), "entity");

        // Create the member binding expressions for excluded properties
        var bindings = typeof(T).GetProperties()
                                 .Where(prop => excludeProperties.Contains(prop.Name))
                                 .Select(prop => Expression.Bind(prop, Expression.Constant(null, prop.PropertyType)));

        // Create the member initializer expression
        var memberInit = Expression.MemberInit(Expression.New(typeof(T)), bindings);

        // Create the lambda expression
        var lambda = Expression.Lambda<Func<T, T>>(memberInit, parameter);

        // Apply the lambda expression to the query
        var result = query.Select(lambda);

        return result.ToList();
    }

    public IEnumerable<T> GetQueryValidation(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.ToList();
    }

    public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
    {
        IIncludableQueryable<T, object> query = null;

        if (includes.Length > 0)
        {
            query = dbSet.Include(includes[0]);
        }
        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }

        return query == null ? dbSet : (IQueryable<T>)query;
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public ICollection<TType> GetSelectedQuery<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes.Length > 0)
        {
            query = query.Include(includes[0]);
        }
        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }

        return query.Select(select).ToList();
    }



    public ICollection<TType> GetSelectedQueryAsNoTracking<TType>(Expression<Func<T, bool>> filter, Expression<Func<T, TType>> select, params Expression<Func<T, object>>[] includes) where TType : class
    {
        //var key = _dbContext.Model.FindEntityType(typeof(T)).GetDeclaredReferencingForeignKeys();
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes.Length > 0)
        {
            query = query.Include(includes[0]);
        }
        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }
        foreach (T entity in query)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
        return query.Select(select).ToList();
    }

    public void Save()
    {
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

   
    public TKey GetEntityIdByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TKey>> keySelector) where TEntity : class
    {
        var entity = _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(condition);
        return entity != null ? keySelector.Compile()(entity) : default(TKey);
    }

    public bool Any(Expression<Func<T, bool>> predicate)
    {
        return dbSet.Any(predicate);
    }

    public List<TKey> GetEntityIdsByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TKey>> keySelector) where TEntity : class
    {
        var entities = _dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
        if (entities == null || entities.Count == 0)
        {
            return new List<TKey>();
        }
        return entities.Select(entity => keySelector.Compile()(entity)).ToList();
    }

    public List<TKey> GetEntityIdsByCondition<TEntity, TKey>(Expression<Func<TEntity, bool>> condition,
                                                             Expression<Func<TEntity, TKey>> keySelector,
                                                             params Expression<Func<TEntity, object>>[] columnSelectors) where TEntity : class

    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsNoTracking();

        foreach (var columnSelector in columnSelectors)
        {
            query = query.Include(columnSelector);
        }

        var entities = query.Where(condition).ToList();

        if (entities == null || entities.Count == 0)
        {
            return new List<TKey>();
        }

        return entities.Select(entity => keySelector.Compile()(entity)).ToList();
    }

    public void UpdateRange(IEnumerable<T> entityList)
    {
        dbSet.UpdateRange(entityList);
    }

    public void ExecuteSqlCommand(string sqlQuery)
    {
        //_dbContext.Database.ExecuteSqlRaw(sqlQuery);
    }

    public IEnumerable<TResult> SelectMany<TResult>(Func<T, IEnumerable<TResult>> selector,
                                                    Expression<Func<T, bool>> predicate = null,
                                                    bool asNoTracking = false)
    {
        var query = dbSet.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query.SelectMany(selector).ToList();
    }


    public TResult GetMaxColumn<TResult>(Expression<Func<T, TResult>> selector)
    {
        TResult? maxVal = dbSet.Max(selector);
        if (maxVal == null)
        {
            return default(TResult);
        }
        return maxVal;
    }

    public TResult GetMinColumn<TResult>(Expression<Func<T, TResult>> selector) => dbSet.Min(selector) ?? default(TResult);
}
