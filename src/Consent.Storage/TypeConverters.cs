using Consent.Domain.Contracts;
using Consent.Domain.Core;
using Consent.Domain.Participants;
using Consent.Domain.Purposes;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Consent.Storage;
internal static class TypeConverters
{
    public static void Configure(ModelConfigurationBuilder configurationBuilder)
    {
        _ = configurationBuilder
            .Properties<UserId>()
            .HaveConversion<IdentifierTypeConverter<UserId>>();
        _ = configurationBuilder
            .Properties<WorkspaceId>()
            .HaveConversion<IdentifierTypeConverter<WorkspaceId>>();
        _ = configurationBuilder
            .Properties<MembershipId>()
            .HaveConversion<IdentifierTypeConverter<MembershipId>>();
        _ = configurationBuilder
            .Properties<PurposeId>()
            .HaveConversion<IdentifierTypeConverter<PurposeId>>();
        _ = configurationBuilder
            .Properties<ParticipantId>()
            .HaveConversion<IdentifierTypeConverter<ParticipantId>>();
        _ = configurationBuilder
            .Properties<ContractId>()
            .HaveConversion<IdentifierTypeConverter<ContractId>>();
        _ = configurationBuilder
            .Properties<ContractVersionId>()
            .HaveConversion<IdentifierTypeConverter<ContractVersionId>>();
        _ = configurationBuilder
            .Properties<ProvisionId>()
            .HaveConversion<IdentifierTypeConverter<ProvisionId>>();
    }
}

public class IdentifierTypeConverter<TIdentifier> : ValueConverter<TIdentifier, int> where TIdentifier : IIdentifier, new()
{
    public IdentifierTypeConverter() : base(v => v.Value, v => new TIdentifier { Value = v })
    {
    }
}
