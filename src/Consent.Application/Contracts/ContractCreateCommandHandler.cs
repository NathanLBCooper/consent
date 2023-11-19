using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Users;
using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Application.Contracts;

public interface IContractCreateCommandHandler : ICommandHandler<ContractCreateCommand, Result<Contract>> { }

public class ContractCreateCommandHandler : IContractCreateCommandHandler
{
    private readonly IContractRepository _contractRepository;
    private readonly IUserRepository _userRepository;
    private readonly ContractCreateCommandValidator _validator = new();

    public ContractCreateCommandHandler(IContractRepository contractRepository, IUserRepository userRepository)
    {
        _contractRepository = contractRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Contract>> Handle(ContractCreateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Result<Contract>.Failure(new ValidationError(validationResult.ToString()));
        }

        // todo go through workspace. Maybe remove CanView and CanEdit from User?
        var user = await _userRepository.Get(command.RequestedBy);
        if (user == null || !user.CanEditWorkspace(command.WorkspaceId))
        {
            return Result<Contract>.Failure(new UnauthorizedError());
        }

        var created = await _contractRepository.Create(new Contract(Guard.NotNull(command.Name), command.WorkspaceId));

        return Result<Contract>.Success(created);
    }
}
