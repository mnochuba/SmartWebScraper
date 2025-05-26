using FluentValidation;
using MediatR;
using SmartWebScraper.Application.Contracts;
using SmartWebScraper.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartWebScraper.Application.Features.Commands.SearchForKeyword;
public class SearchForKeywordCommandHandler(IUnitOfWork unitOfWork, IValidator<SearchForKeywordCommand> validator)
                                            : IRequestHandler<SearchForKeywordCommand, OperationResult<List<int>>>
{

    public async Task<OperationResult<List<int>>> Handle(SearchForKeywordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return OperationResult<List<int>>.Failure(HttpStatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));
        }
        try
        {
            //var searchResults = new List<int>();
            //foreach (var searchPhrase in request.SearchPhrases)
            //{
            //    var saveCommand = new SaveSearchResultCommand(searchPhrase, request.Positions, request.TargetUrl);
            //    var result = await new SaveSearchResultCommandHandler(_unitOfWork, _validator).Handle(saveCommand, cancellationToken);

            //    if (result.IsSuccessful)
            //    {
            //        searchResults.Add(result.Data);
            //    }
            //    else
            //    {
            //        return OperationResult<List<int>>.Failure(result.Code).AddErrors(result.Errors);
            //    }
            //}
            //var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
            //if (saveResult.IsSuccessful)
            //{
            //    return OperationResult<List<int>>.Success(searchResults);
            //}
            return OperationResult<List<int>>.Failure(/*saveResult.Code).AddErrors(saveResult.Errors*/);
        }
        catch (Exception ex)
        {
            return OperationResult<List<int>>.Failure(HttpStatusCode.InternalServerError)
                .AddError($"An error occurred while processing the search: {ex.Message}");
        }
    }

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<SearchForKeywordCommand> _validator = validator;
}
