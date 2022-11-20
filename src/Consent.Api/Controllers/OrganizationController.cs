using Consent.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Consent.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(ILogger<OrganizationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetOrganization")]
        public OrganizationModel Get()
        {
            _logger.LogError("foo");

            return new OrganizationModel { Id = 1, Name = "foo" };
        }
    }
}
