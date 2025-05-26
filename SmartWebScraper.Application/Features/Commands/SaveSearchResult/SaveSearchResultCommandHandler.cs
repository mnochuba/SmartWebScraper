using FluentValidation;
using MediatR;
using SmartWebScraper.Domain.Contracts;
using SmartWebScraper.Domain.Utilities;
using SmartWebScraper.Domain;
using System.Net;

namespace SmartWebScraper.Application.Features.Commands.SaveSearchResult;
public class SaveSearchResultCommandHandler(IUnitOfWork unitOfWork, IValidator<SaveSearchResultCommand> validator) : IRequestHandler<SaveSearchResultCommand, OperationResult<int>>
{
    public async Task<OperationResult<int>> Handle(SaveSearchResultCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return OperationResult<int>.Failure(HttpStatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        try
        {
            var positions = request.Rankings.Keys.ToList();
            var urls = request.Rankings.Values.ToList();
            var searchRsult = new SearchResult(request.SearchPhrase, positions, urls, request.TargetUrl);
            await _unitOfWork.SearchResultRepository.AddAsync(searchRsult, cancellationToken);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result.IsSuccessful)
            {
                return OperationResult<int>.Success(searchRsult.Id);
            }
            return OperationResult<int>.Failure(result.Code).AddErrors(result.Errors);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Failure(HttpStatusCode.InternalServerError)
                .AddError($"An error occurred while saving the search result: {ex.Message}");
        }
    }

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<SaveSearchResultCommand> _validator = validator;
}
