using System;
using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;

namespace Consent.Domain.Contracts;

/*
 * A specific version of a Contract, which contains all wording required for informed consent on one or many provisions 
 */

public class ContractVersion
{
    public ContractVersionId? Id { get; init; }

    public string Name { get; private set; }
    public Result NameSet(string value)
    {
        var isEditable = CheckIsEditable();
        if (isEditable.IsFailure)
        {
            return isEditable;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(new ArgumentError(null, nameof(Name)));
        }

        Name = value;
        return Result.Success();
    }

    // todo is this "Text". Is it something more specific like a introduction?
    public string Text { get; private set; }
    public Result TextSet(string value)
    {
        var isValid = CheckIsEditable();
        if (isValid.IsFailure)
        {
            return isValid;
        }

        Text = value;
        return Result.Success();
    }

    public ContractVersionStatus Status { get; private set; }
    public Result StatusSet(ContractVersionStatus value)
    {
        if (value == Status)
        {
            return Result.Success();
        }

        if (!Enum.IsDefined(typeof(ContractVersionStatus), value))
        {
            return Result.Failure(new ArgumentError(null, nameof(Name)));
        }

        if (value == ContractVersionStatus.Draft)
        {
            return Result.Failure(new InvalidOperationError("Cannot change a non-draft Version back to draft"));
        }

        Status = value;
        return Result.Success();
    }

    private readonly List<Provision> _provisions;
    public IReadOnlyList<Provision> Provisions => _provisions.AsReadOnly();

    public static Result<ContractVersion> New(string name, string text, IEnumerable<Provision> provisions)
    {
        Result result;
        var contractVersion = new ContractVersion(name, text, ContractVersionStatus.Draft, provisions.ToList());

        result = contractVersion.NameSet(name);
        if (result.IsFailure)
        {
            return Result<ContractVersion>.Failure(result.Error);
        }

        result = contractVersion.TextSet(text);
        if (result.IsFailure)
        {
            return Result<ContractVersion>.Failure(result.Error);
        }

        foreach (var p in contractVersion._provisions)
        {
            p.OnAddedToVersion(contractVersion);
        }

        return Result<ContractVersion>.Success(contractVersion);
    }

    private ContractVersion(string name, string text, ContractVersionStatus status, IEnumerable<Provision> provisions)
    {
        Name = name;
        Text = text;
        Status = status;
        _provisions = provisions.ToList();
    }

    private ContractVersion(string name, string text, ContractVersionStatus status) : this(name, text, status, Array.Empty<Provision>())
    {
    }

    public void AddProvisions(params Provision[] provisions)
    {
        _provisions.AddRange(provisions);
        foreach (var p in provisions)
        {
            p.OnAddedToVersion(this);
        }
    }

    private Result CheckIsEditable()
    {
        return Status == ContractVersionStatus.Draft
            ? Result.Success()
            : Result.Failure(new InvalidOperationError("Cannot edit non-draft contract"));
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
