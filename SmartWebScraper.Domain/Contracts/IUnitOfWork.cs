using SmartWebScraper.Domain.Utilities;

namespace SmartWebScraper.Domain.Contracts;
public interface IUnitOfWork
{
    ISearchResultRepository SearchResultRepository { get; }
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}
