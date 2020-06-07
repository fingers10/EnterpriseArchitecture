using Ardalis.GuardClauses;
using AutoMapper;
using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.API.ResourceParameters;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.API.Controllers
{
    /// <summary>
    /// Controller to work on Author
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly Messages _messages;

        /// <summary>
        /// Constructor for Author Controller
        /// </summary>
        /// <param name="messages">Message to Dispatch Command</param>
        public AuthorController(Messages messages)
        {
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        /// <summary>
        /// Get the list of authors
        /// </summary>
        /// <param name="authorsResourceParameters">author resource parameters</param>
        /// <returns>An ActionResult of type IEnumerable of AuthorDto</returns>
        [HttpGet(Name = nameof(GetAuthors))]
        [HttpHead(Name = "HeadAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(
            [FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var list = await _messages.Dispatch(new GetAuthorsListQuery(authorsResourceParameters.MainCategory));
            return Ok(list);
        }

        /// <summary>
        /// Get the author by id
        /// </summary>
        /// <param name="authorId">Id of the author that you want</param>
        /// <returns>An ActionResult of type AuthorDto</returns>
        [HttpGet("{authorId:long:min(1)}", Name = nameof(GetAuthor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        /// <summary>
        /// Create a author
        /// </summary>
        /// <param name="author">The author to create</param>
        /// <param name="mapper">Mapper to map objects</param>
        /// <returns>An ActionResult of type AuthorDto</returns>
        [HttpPost(Name = nameof(CreateAuthor))]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDto))]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto author, [FromServices] IMapper mapper)
        {
            Guard.Against.Null(author, nameof(author));
            Guard.Against.Null(mapper, nameof(mapper));

            var command = new CreateAuthorCommand(author.FirstName, author.LastName, author.DateOfBirth, author.MainCategory, author.Books);

            Result<Author> result = await _messages.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var authorToReturn = mapper.Map<AuthorDto>(result.Value);
            return CreatedAtRoute(nameof(GetAuthor), new { authorId = authorToReturn.Id }, authorToReturn);
        }

        /// <summary>
        /// HTTP Options available on author
        /// </summary>
        /// <returns>returns HTTP Options available for author</returns>
        [HttpOptions(Name = nameof(GetAuthorsOptions))]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        /// <summary>
        /// Delete a author
        /// </summary>
        /// <param name="authorId">The id of the author to be deleted</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{authorId:long:min(1)}", Name = nameof(DeleteAuthor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            Result result = await _messages.Dispatch(new DeleteAuthorCommand(authorId));

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }
    }
}
