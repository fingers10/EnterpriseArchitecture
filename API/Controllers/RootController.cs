using Fingers10.EnterpriseArchitecture.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRoot()
        {
            // create links for root
            var links = new List<LinkDto>
            {
                new LinkDto(Url.Link(nameof(GetRoot), new { }),
              "self",
              "GET"),

                new LinkDto(Url.Link(nameof(AuthorController.GetAuthors), new { }),
              "authors",
              "GET"),

                new LinkDto(Url.Link(nameof(AuthorController.CreateAuthor), new { }),
              "create_author",
              "POST")
            };

            return Ok(links);
        }
    }
}
