using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    private string _name;
    public string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))]
        set
        {
            ValidateName(value).Unwrap();
            _name = value;
        }
    }
    public Result ValidateName(string value) => ValidateName(value, Status);
    private static Result ValidateName(string value, ContractVersionStatus status)
    {
        var isEditable = CheckIsEditable(status);
        if (isEditable.IsFailure)
        {
            return isEditable;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(new ArgumentError(null, nameof(Name)));
        }

        return Result.Success();
    }

    // todo is this "Text". Is it something more specific like a introduction?
    private string _text;
    public string Text
    {
        get => _text;
        [MemberNotNull(nameof(_text))]
        set
        {
            ValidateText(value, Status).Unwrap();
            _text = value;
        }
    }
    public Result ValidateText(string value) => ValidateText(value, Status);
    private static Result ValidateText(string _, ContractVersionStatus status)
    {
        var isValid = CheckIsEditable(status);
        if (isValid.IsFailure)
        {
            return isValid;
        }

        return Result.Success();
    }

    private ContractVersionStatus _status;
    public ContractVersionStatus Status
    {
        get => _status;
        set
        {
            ValidateStatus(value).Unwrap();
            _status = value;
        }
    }
    public Result ValidateStatus(ContractVersionStatus value) => ValidateStatus(value, Status);
    private static Result ValidateStatus(ContractVersionStatus value, ContractVersionStatus status)
    {
        if (value == status)
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

        return Result.Success();
    }

    private readonly List<Provision> _provisions;
    public IReadOnlyList<Provision> Provisions => _provisions.AsReadOnly();

    public static Result ValidateNew(string name, string text, IEnumerable<Provision> _)
    {
        Result result;
        var status = ContractVersionStatus.Draft;

        result = ValidateName(name, status);
        if (result.IsFailure)
        {
            return result;
        }

        result = ValidateText(text, status);
        if (result.IsFailure)
        {
            return result;
        }

        return Result.Success();
    }

    public ContractVersion(string name, string text, ContractVersionStatus status, IEnumerable<Provision> provisions)
    {
        Status = status;
        Name = name;
        Text = text;
        _provisions = provisions.ToList();

        foreach (var p in _provisions)
        {
            p.OnAddedToVersion(this);
        }
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

    private static Result CheckIsEditable(ContractVersionStatus status)
    {
        return status == ContractVersionStatus.Draft
            ? Result.Success()
            : Result.Failure(new InvalidOperationError("Cannot edit non-draft contract"));
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
