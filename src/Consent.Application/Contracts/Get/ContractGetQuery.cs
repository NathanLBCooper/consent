using Consent.Domain.Contracts;
using Consent.Domain.Users;

namespace Consent.Application.Contracts.Get;

public record ContractGetQuery(
    ContractId ContractId,
    UserId RequestedBy
    );
