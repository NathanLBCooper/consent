using System;
using System.Collections.Generic;
using Consent.Domain.Core.Errors;
using Consent.Domain.Users;
using Consent.Domain.Workspaces;
using Shouldly;

namespace Consent.Tests.Workspaces;

public class WorkspaceTest
{
    [Theory]
#pragma warning disable xUnit1012 // <Nullable> does not guarantee no nulls
    [InlineData(null)]
#pragma warning restore xUnit1012
    [InlineData("")]
    [InlineData("  ")]
    public void Cannot_create_workspace_with_empty_name(string name)
    {
        var result = Workspace.Ctor(name, new List<Membership>());
        var error = result.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(Workspace.Name));

        var userResult = Workspace.Ctor(name, new UserId(1));
        var userError = userResult.UnwrapError().ShouldBeOfType<ArgumentError>();
        userError.ParamName.ShouldBe(nameof(Workspace.Name));
    }

    [Fact]
    public void Can_get_user_permissions()
    {
        var userId = new UserId(1);
        var permissions = new List<Permission> { Permission.Edit, Permission.View };
        var workspace = Workspace.Ctor("myworkspace", new List<Membership> {
            Membership.Ctor(userId, permissions ),
            Membership.Ctor(new UserId(2), Membership.SuperUser)
        }).Unwrap();

        workspace.GetUserPermissions(userId).ShouldBe(permissions);
        workspace.GetUserPermissions(new UserId(3)).ShouldBe(Array.Empty<Permission>());
    }

    [Fact]
    public void Users_with_view_permissions_can_view()
    {
        var bob = new UserId(10);
        var eve = new UserId(11);
        var workspace = Workspace.Ctor("myworkspace", new List<Membership> {
            Membership.Ctor(bob, new[] { Permission.View }),
            Membership.Ctor(eve, new[] { Permission.Edit }),
            Membership.Ctor(new UserId(12), Membership.SuperUser)
        }).Unwrap();

        workspace.UserCanView(bob).ShouldBeTrue();
        workspace.UserCanView(eve).ShouldBeFalse();
        workspace.UserCanView(new UserId(13)).ShouldBeFalse();
    }

    [Fact]
    public void Users_with_edit_permissions_can_edit()
    {
        var bob = new UserId(10);
        var eve = new UserId(11);
        var workspace = Workspace.Ctor("myworkspace", new List<Membership> {
            Membership.Ctor(bob, new[] { Permission.Edit }),
            Membership.Ctor(eve, new[] { Permission.Admin }),
            Membership.Ctor(new UserId(12), Membership.SuperUser)
        }).Unwrap();

        workspace.UserCanEdit(bob).ShouldBeTrue();
        workspace.UserCanEdit(eve).ShouldBeFalse();
        workspace.UserCanEdit(new UserId(13)).ShouldBeFalse();
    }

    [Fact]
    public void Creating_a_workspace_with_user_gives_user_all_permissions()
    {
        var userId = new UserId(1);
        var workspace = Workspace.Ctor("myworkspace", userId).Unwrap();

        workspace.GetUserPermissions(userId).ShouldBe(
            new[] {
                Permission.View, Permission.Edit,
                Permission.Admin, Permission.Buyer }
            );
    }

    [Fact]
    public void Cannot_create_workspace_without_a_superUser()
    {
        var emptyResult = Workspace.Ctor("myworkspace", new List<Membership>());
        var emptyError = emptyResult.UnwrapError().ShouldBeOfType<ArgumentError>();
        emptyError.ParamName.ShouldBe(nameof(Workspace.Memberships));

        var nonSuperUserResult = Workspace.Ctor("myworkspace",
            new List<Membership>
            {
                Membership.Ctor(new UserId(1), new List<Permission>
                {
                    Permission.Edit, Permission.View
                })
            });
        var nonSuperUserError = nonSuperUserResult.UnwrapError().ShouldBeOfType<ArgumentError>();
        nonSuperUserError.ParamName.ShouldBe(nameof(Workspace.Memberships));
    }

    [Fact]
    public void Cannot_create_workspace_with_duplicate_user()
    {
        var userId = new UserId(1);
        var result = Workspace.Ctor("myworkspace",
            new List<Membership>
            {
                Membership.Ctor(userId, Membership.SuperUser),
                Membership.Ctor(userId, Membership.SuperUser)
            }
        );

        var error = result.UnwrapError().ShouldBeOfType<ArgumentError>();
        error.ParamName.ShouldBe(nameof(Workspace.Memberships));
    }

    [Fact]
    public void Workspace_created_by_user_ctor_validates_when_externally_created()
    {
        var userId = new UserId(1);
        var workspace = Workspace.Ctor("myworkspace", userId).Unwrap();
        var recreateResult = Workspace.Ctor(workspace.Name, workspace.Memberships);

        _ = recreateResult.Unwrap();
    }
}
