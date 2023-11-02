using Consent.Domain.Contracts;
using Consent.Domain.Users;

namespace Consent.Application.Contracts;

public record ContractGetQuery(
    ContractId ContractId,
    UserId RequestedBy
    );
