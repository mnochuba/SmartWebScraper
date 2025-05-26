using SmartWebScraper.Application.Utilities;

namespace SmartWebScraper.Application.Contracts;
public interface IUnitOfWork
{
    ISearchResultRepository SearchResultRepository { get; }
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}
