using System.Collections.Generic;
using System.Linq;
using Consent.Domain.Core;
using Consent.Domain.Core.Errors;
using Consent.Domain.Workspaces;

namespace Consent.Domain.Contracts;

/*
 * A document that that is presented to a participant for their full or partial agreement
 */

public class Contract : IEntity<ContractId>
{
    public ContractId? Id { get; init; }

    public WorkspaceId WorkspaceId { get; private init; }

    public string Name { get; private set; }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(new ArgumentError(nameof(Name)));
        }

        return Result.Success();
    }

    public Result SetName(string value)
    {
        if (ValidateName(value) is { IsSuccess: false } result)
        {
            return result;
        }

        Name = value;
        return Result.Success();
    }

    private readonly List<ContractVersion> _versions;
    public IReadOnlyList<ContractVersion> Versions => _versions.AsReadOnly();

    public static Result<Contract> Ctor(WorkspaceId workspaceId, string name)
    {
        return Ctor(workspaceId, name, new List<ContractVersion>());
    }

    public static Result<Contract> Ctor(WorkspaceId workspaceId, string name, IEnumerable<ContractVersion> versions)
    {
        if (ValidateName(name) is { IsSuccess: false } nResult)
        {
            return Result<Contract>.Failure(nResult.Error);
        }

        return Result<Contract>.Success(new Contract(workspaceId, name, versions.ToList()));
    }

    private Contract(WorkspaceId workspaceId, string name, List<ContractVersion> versions)
    {
        WorkspaceId = workspaceId;
        Name = name;
        _versions = versions;
    }

    private Contract(WorkspaceId workspaceId, string name) : this(workspaceId, name, new List<ContractVersion>())
    {
    }

    public void AddContractVersions(params ContractVersion[] versions)
    {
        _versions.AddRange(versions);
    }
}

public readonly record struct ContractId(int Value) : IIdentifier;
