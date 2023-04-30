using System.Threading.Tasks;
using Refit;

namespace Consent.Api.Client.Endpoints;

public interface IHealthEndpoint
{
    [Get("/health")]
    Task Get();
}
