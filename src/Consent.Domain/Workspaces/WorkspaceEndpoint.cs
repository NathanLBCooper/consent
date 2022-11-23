using Consent.Domain.UnitOfWork;
using Consent.Domain.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Consent.Domain.Workspaces
{
    public interface IWorkspaceEndpoint
    {
        Task<WorkspaceEntity> WorkspaceCreate(Workspace workspace, Context ctx);
        Task<WorkspaceEntity?> WorkspaceGet(WorkspaceId id, Context ctx);
        Task<WorkspacePermission[]> WorkspacePermissionsGet(WorkspaceId workspaceId, Context ctx);
        Task<WorkspaceMembership[]?> WorkspaceMembersGet(WorkspaceId workspaceId, Context ctx);
    }

    public class WorkspaceEndpoint : IWorkspaceEndpoint
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICreateUnitOfWork _createUnitOfWork;

        public WorkspaceEndpoint(IWorkspaceRepository workspaceRepository, IUserRepository userRepository,
            ICreateUnitOfWork createUnitOfWork)
        {
            _workspaceRepository = workspaceRepository;
            _userRepository = userRepository;
            _createUnitOfWork = createUnitOfWork;
        }

        public async Task<WorkspaceEntity> WorkspaceCreate(Workspace workspace, Context ctx)
        {
            using var uow = _createUnitOfWork.Create();
            var user = await _userRepository.Get(ctx.UserId);
            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            var created = await _workspaceRepository.Create(workspace);

            await _workspaceRepository.PermissionsCreate(ctx.UserId, created.Id,
                new[] { WorkspacePermission.View, WorkspacePermission.Edit, WorkspacePermission.Admin, WorkspacePermission.Buyer });

            await uow.CommitAsync();
            return created;
        }

        public async Task<WorkspaceEntity?> WorkspaceGet(WorkspaceId id, Context ctx)
        {
            using var uow = _createUnitOfWork.Create();

            var permissions = await _workspaceRepository.PermissionsGet(ctx.UserId, id);
            if (!permissions.Contains(WorkspacePermission.View))
            {
                return null; // todo
            }

            return await _workspaceRepository.Get(id);
        }

        public async Task<WorkspacePermission[]> WorkspacePermissionsGet(WorkspaceId workspaceId, Context ctx)
        {
            using var uow = _createUnitOfWork.Create();

            return await _workspaceRepository.PermissionsGet(ctx.UserId, workspaceId);
        }

        public async Task<WorkspaceMembership[]?> WorkspaceMembersGet(WorkspaceId workspaceId, Context ctx)
        {
            using var uow = _createUnitOfWork.Create();

            var requesterPermissions = await _workspaceRepository.PermissionsGet(ctx.UserId, workspaceId);
            if (!requesterPermissions.Contains(WorkspacePermission.Admin))
            {
                return null;
            }

            return await _workspaceRepository.MembershipsGet(workspaceId);
        }
    }
}
