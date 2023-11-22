using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Purposes;
using Consent.Api.Client.Models.Users;
using Consent.Api.Client.Models.Workspaces;
using Consent.Domain.Core;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;
using static Consent.Tests.Builders.EndpointExtensions;

namespace Consent.Tests.Purposes;

[Collection("DatabaseTest")]
public class PurposeControllerTest : IDisposable
{
    private readonly HttpClient _client;
    private readonly IPurposeEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public PurposeControllerTest(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(
            new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IPurposeEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_purpose()
    {
        var user = await CreateUser();
        var workspace = await CreateWorkspace(user);
        var request = new PurposeCreateRequestModelBuilder(workspace.Id).Build();

        void Verify(PurposeModel model)
        {
            model.Name.ShouldBe(request.Name);
            model.Description.ShouldBe(request.Description);
            model.Workspace.Id.ShouldBe(workspace.Id);
            model.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
        }

        var created = await _sut.PurposeCreate(request, user.Id);
        Verify(created);

        var fetched = await _sut.PurposeGet(created.Id, user.Id);
        fetched.Id.ShouldBe(created.Id);
        Verify(fetched);
    }

    [Fact]
    public async Task Cannot_get_a_non_existant_purpose()
    {
        var user = await UserCreate(_userEndpoint);
        var purposeId = -1;

        var fetch = async () => await _sut.PurposeGet(purposeId, user.Id);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_purpose_with_nonexistant_user()
    {
        var creator = await UserCreate(_userEndpoint);
        var workspace = await WorkspaceCreate(_workspaceEndpoint, creator);
        var purpose = await PurposeCreate(_sut, workspace, creator);
        var userId = -1;

        var fetch = async () => await _sut.PurposeGet(purpose.Id, userId);

        var error = await fetch.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_get_purpose_with_user_with_no_permission() // todo without VIEW permissions
    {
        var alice = await UserCreate(_userEndpoint);
        var alicesWorkspace = await WorkspaceCreate(_workspaceEndpoint, alice);
        var alicesPurpose = await PurposeCreate(_sut, alicesWorkspace, alice);
        var eve = await UserCreate(_userEndpoint);

        var eveFetchesAlicesPurpose = async () => await _sut.PurposeGet(alicesPurpose.Id, eve.Id);

        var error = await eveFetchesAlicesPurpose.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_create_purpose_with_nonexistant_user()
    {
        var workspaceCreator = await UserCreate(_userEndpoint);
        var workspace = WorkspaceCreate(_workspaceEndpoint, workspaceCreator);
        var userId = -1;

        var createPurpose = async () => await _sut.PurposeCreate(new PurposeCreateRequestModelBuilder(workspace.Id).Build(), userId);

        var error = await createPurpose.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_create_purpose_with_nonexistant_workspace()
    {
        var user = await UserCreate(_userEndpoint);
        var workspaceId = -1;

        var createPurpose = async () => await _sut.PurposeCreate(new PurposeCreateRequestModelBuilder(workspaceId).Build(), user.Id);

        var error = await createPurpose.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Cannot_create_purpose_with_user_without_edit_permission()  // todo without EDIT permissions
    {
        var alice = await UserCreate(_userEndpoint);
        var alicesWorkspace = await WorkspaceCreate(_workspaceEndpoint, alice);
        var eve = await UserCreate(_userEndpoint);

        var eveCreatesPurposeOnAlicesWorkspace =
            async () => await _sut.PurposeCreate(new PurposeCreateRequestModelBuilder(alicesWorkspace.Id).Build(), eve.Id);

        var error = await eveCreatesPurposeOnAlicesWorkspace.ShouldThrowAsync<ValidationApiException>();
        Guard.NotNull(error.Content).Status.ShouldBe((int)HttpStatusCode.NotFound);
    }

    private async Task<UserModel> CreateUser()
    {
        return await _userEndpoint.UserCreate(new UserCreateRequestModelBuilder().Build());
    }

    private async Task<WorkspaceModel> CreateWorkspace(UserModel user)
    {
        return await _workspaceEndpoint.WorkspaceCreate(
            request: new WorkspaceCreateRequestModelBuilder().Build(),
            userId: user.Id
            );
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
