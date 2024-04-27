using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Consent.Api.Client.Endpoints;
using Consent.Api.Client.Models.Contracts;
using Consent.Tests.Builders;
using Consent.Tests.Infrastructure;
using Refit;
using Shouldly;
using static Consent.Tests.Builders.EndpointExtensions;

namespace Consent.Tests.Contracts;

[Collection("DatabaseTest")]
public class ContractControllerTest_Provision : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest_Provision(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IContractEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_a_provision()
    {
        var user = await UserCreate(_userEndpoint);
        var contract = await ContractCreate(_sut, await WorkspaceCreate(_workspaceEndpoint, user), user);
        var version = await VersionCreate(_sut, contract, user);
        var purposeId = 1; // todo, doesn't validate existance
        var request = new ProvisionCreateRequestModelBuilder(new[] { purposeId }).Build();

        var createdProvision = await _sut.ProvisionCreate(version.Id, request, user.Id);
        Verify(createdProvision);

        var fetchedVersion = await _sut.ContractVersionGet(version.Id, user.Id);
        var fetchedProvision = fetchedVersion.Provisions.Single(p => p.Id == createdProvision.Id);
        Verify(fetchedProvision);

        void Verify(ProvisionModel p)
        {
            p.Text.ShouldBe(request.Text);
            var purpose = p.Purposes.ShouldHaveSingleItem();
            purpose.Id.ShouldBe(purposeId);
            purpose.Href.ShouldBe(null); // todo, no controller
            p.Version.Id.ShouldBe(version.Id);
            p.Version.Href.ShouldBe($"/Contract/version/{version.Id}");
        }
    }

    [Fact]
    public async Task Cannot_create_a_provision_without_any_purposes()
    {
        var user = await UserCreate(_userEndpoint);
        var contract = await ContractCreate(_sut, await WorkspaceCreate(_workspaceEndpoint, user), user);
        var version = await VersionCreate(_sut, contract, user);
        var request = new ProvisionCreateRequestModelBuilder(Array.Empty<int>()).Build(); // todo null

        var createProvision = async () => await _sut.ProvisionCreate(version.Id, request, user.Id);

        var error = await createProvision.ShouldThrowAsync<ApiException>();
        ((int)error.StatusCode).ShouldBe((int)HttpStatusCode.UnprocessableEntity);
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_on_a_non_existant_contract()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_on_a_non_existant_contract_version()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_with_user_without_edit_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
        // todo maybe it should be unauthorized, but if no view permissions as well, notfound
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_with_non_existant_purpose()
    {
        await Task.CompletedTask;
        // todo
    }


    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_provision_with_duplicate_purposes()
    {
        await Task.CompletedTask;
        // todo
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
