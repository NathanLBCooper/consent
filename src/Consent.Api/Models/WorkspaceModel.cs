using Consent.Domain.Workspaces;

namespace Consent.Api.Models
{
    public record WorkspaceModel
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }

    internal static class WorkspaceModelMapper
    {
        public static WorkspaceModel ToModel(this WorkspaceEntity entity)
        {
            return new WorkspaceModel { Id = entity.Id.Value, Name = entity.Name };
        }
    }
}
