using Ardalis.GuardClauses;
using AutoMapper;
using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.API.Helpers;
using Fingers10.EnterpriseArchitecture.API.Models;
using Fingers10.EnterpriseArchitecture.API.ResourceParameters;
using Fingers10.EnterpriseArchitecture.API.Services;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private readonly IMapper _mapper;
        private readonly IPropertyCheckerService _propertyCheckerService;

        /// <summary>
        /// Constructor for Author Controller
        /// </summary>
        /// <param name="messages">Message to Dispatch Command</param>
        /// <param name="mapper">Mapper to map objects</param>
        /// <param name="propertyCheckerService">Service to verify properties</param>
        public AuthorController(Messages messages, IMapper mapper, IPropertyCheckerService propertyCheckerService)
        {
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
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
            if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields))
            {
                return BadRequest($"No field with the name {authorsResourceParameters.Fields} was found.");
            }

            var authorsFromRepo = await _messages.Dispatch(new GetAuthorsListQuery(authorsResourceParameters.MainCategory,
                authorsResourceParameters.PageNumber, authorsResourceParameters.PageSize));

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = CreateLinksForAuthors(authorsResourceParameters,
                authorsFromRepo.HasNext,
                authorsFromRepo.HasPrevious);

            var shapedAuthors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo)
                                .ShapeData(authorsResourceParameters.Fields);

            var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForAuthor((long)authorAsDictionary["Id"], null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedAuthorsWithLinks,
                links
            };

            return Ok(linkedCollectionResource);
        }

        /// <summary>
        /// Get the author by id
        /// </summary>
        /// <param name="authorId">Id of the author that you want</param>
        /// <param name="fields">Fields to return</param>
        /// <param name="mediaType">Accept media type</param>
        /// <returns>An ActionResult of type AuthorDto</returns>
        [HttpGet("{authorId:long:min(1)}", Name = nameof(GetAuthor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDto))]
        [Produces("application/vnd.fingers10.hateoas+json")]
        public async Task<IActionResult> GetAuthor(int authorId, string fields,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }

            var authorFromRepo = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (authorFromRepo is null)
            {
                return NotFound();
            }

            if (parsedMediaType.MediaType == "application/vnd.fingers10.hateoas+json")
            {

                var links = CreateLinksForAuthor(authorId, fields);

                var linkedResourceToReturn = _mapper.Map<AuthorDto>(authorFromRepo).ShapeData(fields)
                                             as IDictionary<string, object>;

                linkedResourceToReturn.Add("links", links);

                return Ok(linkedResourceToReturn);
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo).ShapeData(fields));
        }

        /// <summary>
        /// Create a author
        /// </summary>
        /// <param name="author">The author to create</param>
        /// <returns>An ActionResult of type AuthorDto</returns>
        [HttpPost(Name = nameof(CreateAuthor))]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDto))]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorDto author)
        {
            Guard.Against.Null(author, nameof(author));

            var command = new CreateAuthorCommand(author.FirstName, author.LastName, author.DateOfBirth, author.MainCategory, author.Books);

            Result<Author> result = await _messages.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var authorToReturn = _mapper.Map<AuthorDto>(result.Value);
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

        private string CreateAuthorsResourceUri(
           AuthorsResourceParameters authorsResourceParameters,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          fields = authorsResourceParameters.Fields,
                          orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                    new
                    {
                        fields = authorsResourceParameters.Fields,
                        orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            }

        }

        private IEnumerable<LinkDto> CreateLinksForAuthor(long authorId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                  new LinkDto(Url.Link(nameof(GetAuthor), new { authorId }),
                  "self",
                  "GET"));
            }
            else
            {
                links.Add(
                  new LinkDto(Url.Link(nameof(GetAuthor), new { authorId, fields }),
                  "self",
                  "GET"));
            }

            links.Add(
               new LinkDto(Url.Link(nameof(DeleteAuthor), new { authorId }),
               "delete_author",
               "DELETE"));

            links.Add(
                new LinkDto(Url.Link(nameof(BookController.CreateBookForAuthor), new { authorId }),
                "create_book_for_author",
                "POST"));

            links.Add(
               new LinkDto(Url.Link(nameof(BookController.GetBooksForAuthor), new { authorId }),
               "books",
               "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForAuthors(
            AuthorsResourceParameters authorsResourceParameters,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>
            {

                // self 
                new LinkDto(CreateAuthorsResourceUri(
                   authorsResourceParameters, ResourceUriType.Current)
               , "self", "GET")
            };

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateAuthorsResourceUri(
                      authorsResourceParameters, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAuthorsResourceUri(
                        authorsResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }
    }
}
