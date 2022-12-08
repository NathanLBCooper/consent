using System;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Shouldly;

namespace Consent.Tests.Workspaces;

public class WorkspaceTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_workspace_with_empty_name(string name)
    {
        var action = () => new Workspace(name, Array.Empty<WorkspaceMembership>());
        _ = action.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_get_user_permissions()
    {
        var userId = new UserId(1);
        var permissions = new[] { WorkspacePermission.Edit, WorkspacePermission.View };
        var workspace = new Workspace("myworkspace", new WorkspaceMembership[] {
            new(new UserId(1),permissions )
        });

        workspace.GetUserPermissions(userId).ShouldBe(permissions);
        workspace.GetUserPermissions(new UserId(2)).ShouldBe(Array.Empty<WorkspacePermission>());
    }

    [Fact]
    public void Cannot_create_workspace_without_a_user_with_all_permissions()
    {
        // todo add validation rule for membership
    }
}

internal class WorkspaceBuilder
{
    // todo
}
