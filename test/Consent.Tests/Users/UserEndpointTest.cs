using Consent.Domain;
using Consent.Domain.Users;
using Consent.Storage.Users;
using Consent.Tests.StorageContext;
using Shouldly;
using System.Threading.Tasks;

namespace Consent.Tests.Users
{
    [Collection("DatabaseTest")]
    public class UserEndpointTest
    {
        private readonly DatabaseFixture _fixture;
        private readonly UserEndpoint _sut;

        public UserEndpointTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            var userRepository = new UserRepository(_fixture.GetConnection);
            _sut = new UserEndpoint(userRepository, _fixture.CreateUnitOfWork);
        }

        [Fact]
        public async Task Can_create_and_get_a_user()
        {
            var user = new User("somename");
            var created = await _sut.UserCreate(user);

            created.ShouldNotBeNull();
            created.Name.ShouldBe(user.Name);

            var fetched = await _sut.UserGet(new Context { UserId = created.Id });
            fetched.ShouldNotBeNull();
            fetched.Id.ShouldBe(created.Id);
            fetched.Name.ShouldBe(created.Name);
        }

    }
}
