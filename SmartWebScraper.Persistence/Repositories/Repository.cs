using Microsoft.EntityFrameworkCore;
using SmartWebScraper.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWebScraper.Persistence.Repositories;
public class Repository<TEntity>(AppDbContext dbContext) : IRepository<TEntity> where TEntity : class
{
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllsync(CancellationToken cancellationToken = default)
    {
        var entities = await dbContext.Set<TEntity>().AsNoTracking()
                                      .ToListAsync(cancellationToken);
        return entities.AsReadOnly();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var entities = await dbContext.Set<TEntity>().Where(predicate)
                                      .AsNoTracking().ToListAsync();
        return entities.AsReadOnly();
    }
}
