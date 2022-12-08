using System.Threading.Tasks;
using Consent.Domain.UnitOfWork;

namespace Consent.Domain.Users;

public interface IUserEndpoint
{
    Task<UserEntity> UserCreate(User user);
    Task<UserEntity?> UserGet(Context ctx);
}

public class UserEndpoint : IUserEndpoint
{
    private readonly IUserRepository _userRepository;
    private readonly ICreateUnitOfWork _createUnitOfWork;

    public UserEndpoint(IUserRepository userRepository, ICreateUnitOfWork createUnitOfWork)
    {
        _userRepository = userRepository;
        _createUnitOfWork = createUnitOfWork;
    }

    public async Task<UserEntity> UserCreate(User user)
    {
        using var uow = _createUnitOfWork.Create();
        var created = await _userRepository.Create(user);
        await uow.CommitAsync();
        return created;
    }

    public async Task<UserEntity?> UserGet(Context ctx)
    {
        using var uow = _createUnitOfWork.Create();
        return await _userRepository.Get(ctx.UserId);
    }
}
