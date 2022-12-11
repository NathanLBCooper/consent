using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consent.Api.Controllers;
using Consent.Api.Models;
using Consent.Storage.Users;
using Consent.Tests.StorageContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Shouldly;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest
{
    private readonly UserController _sut;
    private readonly HeaderTestHelper _headerTestHelper;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var unitOfWorkContext = fixture.CreateUnitOfWorkContext();

        var userRepository = new UserRepository(unitOfWorkContext);
        _sut = new UserController(new NullLogger<UserController>(), userRepository, unitOfWorkContext)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };

        _headerTestHelper = new HeaderTestHelper(_sut);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var request = UserRequest();
        var created = (await CreateUser(request)).GetValue();
        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);

        var fetched = (await GetUser(created.Id)).GetValue();
        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    [Fact]
    public async Task Etags_work()
    {
        const string etag = "2f90b0856f9efa508a4fe8c7348a1336858f45c9";

        var request = UserRequest();
        var created = (await CreateUser(request)).GetValue();
        _ = created.ShouldNotBeNull();
        _headerTestHelper.GetRecordedHeader(HeaderNames.ETag).ShouldBe(etag);

        var fetched = (await GetUser(created.Id)).GetValue();
        _ = fetched.ShouldNotBeNull();
        _headerTestHelper.GetRecordedHeader(HeaderNames.ETag).ShouldBe(etag);

        var result = (await GetUser(created.Id, etag)).Result as StatusCodeResult;
        _ = result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(304);
    }

    private UserCreateRequestModel UserRequest([CallerMemberName] string callerName = "")
    {
        return new UserCreateRequestModel { Name = $"{callerName}-workspace" };
    }

    private async Task<ActionResult<UserModel>> CreateUser(UserCreateRequestModel request)
    {
        var response = await _sut.UserCreate(request);
        _headerTestHelper.RecordLastHeaders();
        _headerTestHelper.ClearHeaders();
        return response;
    }

    private async Task<ActionResult<UserModel>> GetUser(int userId, string? ifNoneMatch = null)
    {
        var response = await _sut.UserGet(userId, ifNoneMatch);
        _headerTestHelper.RecordLastHeaders();
        _headerTestHelper.ClearHeaders();
        return response;
    }
}
