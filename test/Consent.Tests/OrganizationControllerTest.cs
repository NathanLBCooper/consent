using Consent.Api.Controllers;
using Consent.Api.Models;
using Consent.Domain.Accounts;
using Consent.Storage.Organizations;
using Consent.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Threading.Tasks;

namespace Consent.Api.Tests
{
    [Collection("DatabaseTest")]
    public class OrganizationControllerTest
    {
        private readonly DatabaseFixture _fixture;

        private readonly OrganizationController _controller;
        private readonly OrganizationEndpoint _endpoint;
        private readonly OrganizationRepository _repository;

        public OrganizationControllerTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repository = new OrganizationRepository(_fixture.GetConnection);
            _endpoint = new OrganizationEndpoint(_repository, _fixture.CreateUnitOfWork);
            _controller = new OrganizationController(new NullLogger<OrganizationController>(), _endpoint);
        }

        [Fact]
        public async Task Test1()
        {
            var result = await _controller.Get(1);
            result.Result.ShouldBeOfType<NotFoundResult>();
        }
    }

    internal static class ActionResultExtentions
    {
        public static TValue? GetValue<TValue>(this ActionResult<TValue> result) where TValue : class
        {
            return (result.Result as OkObjectResult)?.Value as TValue;
        }
    }
}
