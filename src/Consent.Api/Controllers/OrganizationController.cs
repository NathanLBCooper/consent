using Consent.Api.Models;
using Consent.Domain.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Consent.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IOrganizationEndpoint _organizationEndpoint;
        private readonly OrganizationCreateRequestModelValidator _organizationCreateRequestModelValidator = new();

        public OrganizationController(ILogger<OrganizationController> logger, IOrganizationEndpoint organizationEndpoint)
        {
            _logger = logger;
            _organizationEndpoint = organizationEndpoint;
        }

        [HttpGet("{id}", Name = "GetOrganization")]
        public async Task<ActionResult<OrganizationModel>> Get(int id)
        {
            var organization = await _organizationEndpoint.Get(id);

            if (organization == null) return NotFound();

            return Ok(organization.ToModel());
        }

        [HttpPost(Name = "CreateOrganization")]
        public async Task<ActionResult<OrganizationModel>> Create(OrganizationCreateRequestModel request)
        {
            var validationResult = _organizationCreateRequestModelValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.ToString());
            }

            if (request?.Name == null) return Problem();

            var organization = await _organizationEndpoint.Create(request.Name);

            return Ok(organization.ToModel());
        }
    }
}
