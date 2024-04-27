using System;
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
public class ContractControllerTest_Version : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest_Version(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IContractEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_contract_version()
    {
        var user = await UserCreate(_userEndpoint);
        var contract = await ContractCreate(_sut, await WorkspaceCreate(_workspaceEndpoint, user), user);
        var request = new ContractVersionCreateRequestModelBuilder().Build();

        var createdVersion = await _sut.ContractVersionCreate(contract.Id, request, user.Id);
        Verify(createdVersion);

        var fetchedVersion = await _sut.ContractVersionGet(createdVersion.Id, user.Id);
        fetchedVersion.Id.ShouldBe(createdVersion.Id);
        Verify(fetchedVersion);

        void Verify(ContractVersionModel v)
        {
            v.Contract.Id.ShouldBe(contract.Id);
            v.Contract.Href.ShouldBe($"/Contract/{contract.Id}");
            v.Name.ShouldBe(v.Name);
            v.Text.ShouldBe(v.Text);
            v.Status.ShouldBe(ContractVersionStatusModel.Draft);
            v.Provisions.ShouldBeEmpty();
        }
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_non_existant_contract_version()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_version_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_version_with_user_without_view_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_on_a_non_existant_contract()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_version_with_user_without_edit_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
        // todo maybe it should be unauthorized, but if no view permissions as well, notfound
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
