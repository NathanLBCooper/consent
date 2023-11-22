using FluentValidation;

namespace Consent.Application.Purposes.Create;

public class PurposeCreateCommandValidator : AbstractValidator<PurposeCreateCommand>
{
    public PurposeCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        // todo
    }
}
