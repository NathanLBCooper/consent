namespace Consent.Api.Client.Models.Contracts;

public record ProvisionCreateRequestModel(
    string? Text, int[]? PurposeIds
    );
