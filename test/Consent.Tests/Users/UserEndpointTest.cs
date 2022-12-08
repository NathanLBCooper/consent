using System.Threading.Tasks;
using Consent.Domain;
using Consent.Domain.Users;
using Consent.Storage.Users;
using Consent.Tests.StorageContext;
using Shouldly;

namespace Consent.Tests.Users;

[Collection("DatabaseTest")]
public class UserEndpointTest
{
    private readonly UserEndpoint _sut;

    public UserEndpointTest(DatabaseFixture fixture)
    {
        var unitOfWorkContext = fixture.CreateUnitOfWorkContext();
        var userRepository = new UserRepository(unitOfWorkContext);
        _sut = new UserEndpoint(userRepository, unitOfWorkContext);
    }

    [Fact]
    public async Task Can_create_and_get_a_user()
    {
        var user = new User("somename");
        var created = await _sut.UserCreate(user);

        _ = created.ShouldNotBeNull();
        created.Name.ShouldBe(user.Name);

        var fetched = await _sut.UserGet(new Context { UserId = created.Id });
        _ = fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Name.ShouldBe(created.Name);
    }
}
