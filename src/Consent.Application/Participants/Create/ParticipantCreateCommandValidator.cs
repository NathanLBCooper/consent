using FluentValidation;

namespace Consent.Application.Participants.Create;

public class ParticipantCreateCommandValidator : AbstractValidator<ParticipantCreateCommand>
{
    public ParticipantCreateCommandValidator()
    {
        _ = RuleFor(q => q).NotEmpty();
        // todo
    }
}
