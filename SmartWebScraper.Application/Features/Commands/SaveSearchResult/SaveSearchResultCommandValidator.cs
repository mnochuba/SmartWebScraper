using FluentValidation;

namespace SmartWebScraper.Application.Features.Commands.SaveSearchResult;
public class SaveSearchResultCommandValidator : AbstractValidator<SaveSearchResultCommand>
{
    public SaveSearchResultCommandValidator()
    {
        RuleFor(x => x.SearchPhrase)
            .NotEmpty().WithMessage("Search phrase is required.")
            .MaximumLength(200).WithMessage("Search phrase is too long.");

        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("Target URL is required.")
            .MaximumLength(200).WithMessage("Target URL is too long.")
            .Must(url => Uri.IsWellFormedUriString("https://" + url, UriKind.Absolute))
            .WithMessage("Target URL must be valid.");

        RuleFor(x => x.Rankings)
            .NotNull().WithMessage("Positions are required.")
            .Must(p => p.All(pos => pos.Key > 0 && pos.Key <= 100))
            .WithMessage("Positions must be between 1 and 100.");
    }
}
