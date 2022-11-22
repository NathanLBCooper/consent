using Consent.Domain.Workspaces;
using Shouldly;
using System;

namespace Consent.Tests.Workspaces
{
    public class WorkspaceTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Cannot_create_workspace_with_empty_name(string name)
        {
            var action = () => new Workspace(name);
            action.ShouldThrow<ArgumentException>();
        }
    }
}
