using Consent.Domain.Contracts;
using Consent.Domain.Users;

namespace Consent.Application.Contracts.VersionCreate;

public record ContractVersionCreateCommand(
    string? Name,
    string? Text,
    ContractId ContractId,
    UserId RequestedBy
    );
