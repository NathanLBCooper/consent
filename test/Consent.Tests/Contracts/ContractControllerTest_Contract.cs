﻿using System;
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
public class ContractControllerTest_Contract : IDisposable
{
    private readonly HttpClient _client;
    private readonly IContractEndpoint _sut;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWorkspaceEndpoint _workspaceEndpoint;

    public ContractControllerTest_Contract(DatabaseFixture fixture)
    {
        var factory = new TestWebApplicationFactory(new InMemoryConfigurationBuilder() { SqlSettings = fixture.SqlSettings }.Build());
        _client = factory.CreateClient();
        _sut = RestService.For<IContractEndpoint>(_client);
        _userEndpoint = RestService.For<IUserEndpoint>(_client);
        _workspaceEndpoint = RestService.For<IWorkspaceEndpoint>(_client);
    }

    [Fact]
    public async Task Can_create_and_get_a_contract()
    {
        var user = await UserCreate(_userEndpoint);
        var workspace = await WorkspaceCreate(_workspaceEndpoint, user);
        var request = new ContractCreateRequestModelBuilder(workspace.Id).Build();

        var createdContract = await _sut.ContractCreate(request, user.Id);
        Verify(createdContract);

        var fetchedContract = await _sut.ContractGet(createdContract.Id, user.Id);
        fetchedContract.Id.ShouldBe(createdContract.Id);
        Verify(fetchedContract);

        void Verify(ContractModel c)
        {
            c.Name.ShouldBe(request.Name);
            c.Workspace.Id.ShouldBe(workspace.Id);
            c.Workspace.Href.ShouldBe($"/Workspace/{workspace.Id}");
            c.Versions.ShouldBeEmpty();
        }
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_non_existant_contract()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_get_a_contract_with_user_without_view_permissions_on_workspace()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_with_a_non_existant_user()
    {
        await Task.CompletedTask;
        // todo
    }

    [Fact]
    public async Task Cannot_create_a_contract_on_a_nonexistant_workspace()
    {
        var user = await UserCreate(_userEndpoint);
        var request = new ContractCreateRequestModelBuilder(-1).Build();

        var create = async () => await _sut.ContractCreate(request, user.Id);

        var error = await create.ShouldThrowAsync<ApiException>();
        ((int)error.StatusCode).ShouldBe((int)HttpStatusCode.NotFound);
    }

    [Fact(Skip = "Unimplemented")]
    public async Task Cannot_create_a_contract_with_user_without_edit_permissions_on_workspace()
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
