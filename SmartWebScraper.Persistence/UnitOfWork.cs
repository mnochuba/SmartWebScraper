using Microsoft.Extensions.Logging;
using SmartWebScraper.Application.Contracts;
using SmartWebScraper.Application.Utilities;
using SmartWebScraper.Persistence.Repositories;
using System.Net;

namespace SmartWebScraper.Persistence;
public class UnitOfWork(AppDbContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger<UnitOfWork> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private ISearchResultRepository? _searchResultRepository;
    public ISearchResultRepository SearchResultRepository =>
        _searchResultRepository ??= new SearchResultRepository(_dbContext);

    public async Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = "Error occured while saving changes.";
            _logger.LogWarning(message: errorMessage, ex);

            return OperationResult.Failure(HttpStatusCode.InternalServerError)
                .AddError(errorMessage);
        }
    }
}
