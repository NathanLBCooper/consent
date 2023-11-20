namespace Consent.Api.Client.Models.Contracts;

public record ProvisionModel(
    int Id, string Text, ResourceLink[] Purposes, ResourceLink Version
    );
