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

    private Result InitName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(new ArgumentError(nameof(Name)));
        }

        Name = value;
        return Result.Success();
    }
    public Result SetName(string value)
    {
        if (FailIfNotDraft() is { IsSuccess: false } dResult)
        {
            return dResult;
        }

        return InitName(value);
    }

    // todo is this "Text". Is it something more specific like a introduction?
    public string Text { get; private set; }

    public Result SetText(string value)
    {
        if (FailIfNotDraft() is { IsSuccess: false } dResult)
        {
            return dResult;
        }

        Text = value;
        return Result.Success();
    }

    public ContractVersionStatus Status { get; private set; }

    public Result SetStatus(ContractVersionStatus value)
    {
        if (value == Status)
        {
            return Result.Success();
        }

        if (!Enum.IsDefined(typeof(ContractVersionStatus), value))
        {
            return Result.Failure(new ArgumentError(nameof(Status)));
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

    private static Result<ContractVersion> Ctor(string name, string text, ContractVersionStatus status,
        IEnumerable<Provision> provisions)
    {
        var pr = provisions.ToList();
        var @new = new ContractVersion(null!, text, default, pr);

        if (@new.InitName(name) is { IsSuccess: false } nResult)
        {
            return Result<ContractVersion>.Failure(nResult.Error);
        }

        if (@new.SetStatus(status) is { IsSuccess: false } sResult)
        {
            return Result<ContractVersion>.Failure(sResult.Error);
        }

        foreach (var p in pr)
        {
            p.OnAddedToVersion(@new);
        }

        return Result<ContractVersion>.Success(@new);
    }

    public static Result<ContractVersion> Ctor(string name, string text, IEnumerable<Provision> provisions)
    {
        return Ctor(name, text, ContractVersionStatus.Draft, provisions);
    }

    public static Result<ContractVersion> Ctor(string name, string text, ContractVersionStatus status)
    {
        return Ctor(name, text, status, new List<Provision>());
    }

    private ContractVersion(string name, string text, ContractVersionStatus status, List<Provision> provisions)
    {
        Name = name;
        Text = text;
        Status = status;
        _provisions = provisions;
    }

    private ContractVersion(string name, string text, ContractVersionStatus status) : this(name, text, status,
        new List<Provision>())
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

    private Result FailIfNotDraft()
    {
        if (Status != ContractVersionStatus.Draft)
        {
            return Result.Failure(new InvalidOperationError("Cannot mutate a non-draft Version"));
        }

        return Result.Success();
    }
}

public readonly record struct ContractVersionId(int Value) : IIdentifier;
