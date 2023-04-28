using System.Threading.Tasks;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Api.Contracts;
using Consent.Api.Users;
using Consent.Api.Workspaces;
using Consent.Domain;
using Consent.Storage.Contacts;
using Consent.Storage.Users;
using Consent.Storage.Workspaces;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Consent.Tests.Contracts;

[Collection("DatabaseTest")]
public class ContractControllerTest
{
    private readonly ContractController _sut;
    private readonly UserControllerTestWrapper _userController;
    private readonly WorkspaceController _workspaceController;

    public ContractControllerTest(DatabaseFixture fixture)
    {
        var userRepository = new UserRepository(fixture.UserDbContext);
        var workspaceRepository = new WorkspaceRepository(fixture.WorkspaceDbContext);
        var contractRepository = new ContractRepository(fixture.ContractDbContext);

        _userController = new UserControllerTestWrapper(new UserController(new NullLogger<UserController>(), new FakeLinkGenerator(), userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        });

        _workspaceController = new WorkspaceController(
            new NullLogger<WorkspaceController>(), workspaceRepository, userRepository
        );

        _sut = new ContractController(new NullLogger<ContractController>(), new FakeLinkGenerator(), contractRepository, userRepository)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    [Fact]
    public async Task Can_create_and_get_a_contract()
    {
        var (user, workspace) = await Setup();
        var request = new ContractCreateRequestModelBuilder(workspace.Id).Build();


        var created = (await _sut.ContractCreate(request, user.Id)).GetValue();

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);
        created.Workspace.Id.ShouldBe(workspace.Id);


        var fetched = (await _sut.ContractGet(created.Id, user.Id, null)).GetValue();

        _ = fetched.ShouldNotBeNull();
        fetched.Name.ShouldBe(request.Name);
        fetched.Workspace.Id.ShouldBe(workspace.Id);
    }

    [Fact]
    public async Task Etags_work()
    {
        await Task.CompletedTask;
        // todo
    }

    private async Task<(UserModel user, WorkspaceModel workspace)> Setup()
    {
        var user = Guard.NotNull((await _userController.UserCreate(
                new UserCreateRequestModelBuilder().Build()
                )).GetValue());

        var workspace = Guard.NotNull((await _workspaceController.WorkspaceCreate(
            request: new WorkspaceCreateRequestModelBuilder().Build(),
            userId: user.Id
            )).GetValue());

        return (user, workspace);
    }
}
