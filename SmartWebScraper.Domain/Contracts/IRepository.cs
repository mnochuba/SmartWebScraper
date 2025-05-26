using System.Linq.Expressions;

namespace SmartWebScraper.Domain.Contracts;
public interface IRepository<TEntity> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}
