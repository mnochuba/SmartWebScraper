using FluentValidation;

namespace SmartWebScraper.Application.Features.Commands.SearchForKeyword;
public class SearchForKeywordCommandValidator : AbstractValidator<SearchForKeywordCommand>
{
    public SearchForKeywordCommandValidator()
    {
        RuleFor(x => x.SearchPhrase)
            .NotEmpty().WithMessage("Search phrase is required.")
            .MaximumLength(200).WithMessage("Search phrase is too long.");
        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("Target URL is required.")
            .MaximumLength(200).WithMessage("Target URL is too long.")
            .Must(url => Uri.IsWellFormedUriString("https://" + url, UriKind.Absolute))
            .WithMessage("Target URL must be valid.");
    }
}
