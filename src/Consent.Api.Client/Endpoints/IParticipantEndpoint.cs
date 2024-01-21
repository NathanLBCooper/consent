using System.Threading.Tasks;
using Consent.Api.Client.Models.Participants;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IParticipantEndpoint
{
    [Get("/participant/{id}")]
    Task<ParticipantModel> ParticipantGet(int id, [Header("userId")] int userId);

    [Post("/participant")]
    Task<ParticipantModel> ParticipantCreate([Body] ParticipantCreateRequestModel request, [Header("userId")] int userId);
}
