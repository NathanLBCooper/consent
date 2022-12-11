﻿using System;
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
        var ctor = () => new Workspace(name, Array.Empty<WorkspaceMembership>());
        _ = ctor.ShouldThrow<ArgumentException>();

        var user_ctor = () => new Workspace(name, new UserId(1));
        _ = user_ctor.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Can_get_user_permissions()
    {
        var userId = new UserId(1);
        var permissions = new[] { WorkspacePermission.Edit, WorkspacePermission.View };
        var workspace = new Workspace("myworkspace", new WorkspaceMembership[] {
            new(userId, permissions ),
            new(new UserId(2), WorkspaceMembership.SuperUser)
        });

        workspace.GetUserPermissions(userId).ShouldBe(permissions);
        workspace.GetUserPermissions(new UserId(3)).ShouldBe(Array.Empty<WorkspacePermission>());
    }

    [Fact]
    public void Creating_a_workspace_with_user_gives_user_all_permissions()
    {
        var userId = new UserId(1);
        var workspace = new Workspace("myworkspace", userId);

        workspace.GetUserPermissions(userId).ShouldBe(
            new[] {
                WorkspacePermission.View, WorkspacePermission.Edit,
                WorkspacePermission.Admin, WorkspacePermission.Buyer }
            );
    }

    [Fact]
    public void Cannot_create_workspace_without_a_superUser()
    {
        var empty = () => new Workspace("myworkspace", Array.Empty<WorkspaceMembership>());
        var nonSuper = () => new Workspace("myworkspace",
            new[] { new WorkspaceMembership(new UserId(1), new[] { WorkspacePermission.Edit, WorkspacePermission.View }) }
            );

        _ = empty.ShouldThrow<ArgumentException>();
        _ = nonSuper.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Cannot_create_workspace_with_duplicate_user()
    {
        var duplicate = () => new Workspace("myworkspace",
            new[] { new WorkspaceMembership(new UserId(1), WorkspaceMembership.SuperUser),
                new WorkspaceMembership(new UserId(1), WorkspaceMembership.SuperUser) }
            );

        _ = duplicate.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Workspace_created_by_user_ctor_validates_when_externally_created()
    {
        var userId = new UserId(1);
        var workspace = new Workspace("myworkspace", userId);
        var recreate = () => new Workspace(workspace.Name, workspace.Memberships);

        _ = recreate.ShouldNotThrow();
    }
}
