using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Consent.Api.Controllers;
using Consent.Api.Models;
using Consent.Storage.Users;
using Consent.Tests.StorageContext;
using Shouldly;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserControllerTest
{
    private readonly UserController _sut;

    public UserControllerTest(DatabaseFixture fixture)
    {
        var unitOfWorkContext = fixture.CreateUnitOfWorkContext();

        var userRepository = new UserRepository(unitOfWorkContext);
        _sut = new UserController(new NullLogger<UserController>(), userRepository, unitOfWorkContext);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var request = UserRequest();
        var created = (await _sut.UserCreate(request)).GetValue<UserModel>();

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(request.Name);

        var fetched = (await _sut.UserGet(created.Id)).GetValue<UserModel>(); ;
        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }

    private UserCreateRequestModel UserRequest([CallerMemberName] string callerName = "")
    {
        return new UserCreateRequestModel { Name = $"{callerName}-workspace" };
    }
}
