using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Purposes;

namespace Consent.Domain.Contracts;

/*
 * A yes or no choice to accept one or many purposes. May not contain all information required for informed consent
 */
public class Provision
{
    public ProvisionId? Id { get; init; }

    public ContractVersion? ContractVersion { get; private set; }

    public string Text { get; private set; }

    private static Result ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Result.Failure(new ArgumentError(nameof(Text)));
        }

        return Result.Success();
    }

    public Result SetText(string value)
    {
        if (FailIfNotDraft() is { IsSuccess: false } dResult)
        {
            return dResult;
        }

        if (ValidateText(value) is { IsSuccess: false } tResult)
        {
            return tResult;
        }

        Text = value;
        return Result.Success();
    }

    public ImmutableList<PurposeId> PurposeIds { get; private set; }

    private static Result ValidatePurposeIds(ImmutableList<PurposeId> purposeIds)
    {
        if (purposeIds.IsEmpty)
        {
            return Result.Failure(new ArgumentError(nameof(PurposeIds), "Cannot be empty"));
        }

        return Result.Success();
    }

    public static Result<Provision> Ctor(string text, IEnumerable<PurposeId> purposeIds)
    {
        if (ValidateText(text) is { IsSuccess: false } tResult)
        {
            return Result<Provision>.Failure(tResult.Error);
        }

        var p = purposeIds.ToImmutableList();
        if (ValidatePurposeIds(p) is { IsSuccess: false } pResult)
        {
            return Result<Provision>.Failure(pResult.Error);
        }

        return Result<Provision>.Success(new Provision(text, p));
    }

    private Provision(string text, ImmutableList<PurposeId> purposeIds)
    {
        Text = text;
        PurposeIds = purposeIds;
    }

    private Provision(string text) : this(text, ImmutableList<PurposeId>.Empty)
    {
    }

    public void OnAddedToVersion(ContractVersion version)
    {
        if (ContractVersion is not null)
        {
            throw new InvalidOperationException("Provision is already attached to a version");
        }

        ContractVersion = version;
    }

    public Result AddPurposeIds(IEnumerable<PurposeId> purposeIds)
    {
        if (FailIfNotDraft() is { IsSuccess: false } dResult)
        {
            return dResult;
        }

        var value = PurposeIds.Concat(purposeIds).ToImmutableList();
        // skip validation, cannot get invalid list (empty) via concat

        PurposeIds = value;
        return Result.Success();
    }

    private Result FailIfNotDraft()
    {
        if (ContractVersion is not null && ContractVersion.Status != ContractVersionStatus.Draft)
        {
            return Result.Failure(new InvalidOperationError("Cannot mutate a non-draft Version"));
        }

        return Result.Success();
    }
}

public readonly record struct ProvisionId(int Value) : IIdentifier;
