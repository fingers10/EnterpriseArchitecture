using Ardalis.GuardClauses;
using AutoMapper;
using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
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
    /// Controller to work on Book
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/authors/{authorId:long:min(1)}/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly Messages _messages;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for Book Controller
        /// </summary>
        /// <param name="messages">Message to Dispatch Command</param>
        /// <param name="mapper">mapper to map objects</param>
        public BookController(Messages messages, IMapper mapper)
        {
            _messages = messages ?? throw new ArgumentNullException(nameof(messages));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get the list of books for author
        /// </summary>
        /// <param name="authorId">The Author Id for which books are needed</param>
        /// <returns>An ActionResult of type IEnumerable of BookDto</returns>
        [HttpGet(Name = nameof(GetBooksForAuthor))]
        [HttpHead(Name = "HeadBooks")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksForAuthor(long authorId)
        {
            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            var books = await _messages.Dispatch(new GetBooksListQuery(authorId));
            return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        /// <summary>
        /// Get the book by id
        /// </summary>
        /// <param name="authorId">Id of the author that you want</param>
        /// <param name="bookId">Id of the book that you want</param>
        /// <returns>An ActionResult of type BookDto</returns>
        [HttpGet("{bookId:long:min(1)}", Name = nameof(GetBookForAuthor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookDto>> GetBookForAuthor(long authorId, long bookId)
        {
            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            var bookForAuthorFromRepo = await _messages.Dispatch(new GetBookQuery(authorId, bookId));

            if (bookForAuthorFromRepo == null)
            {
                return NotFound($"No book with id {bookId} was found for author with id {authorId}.");
            }

            return Ok(_mapper.Map<BookDto>(bookForAuthorFromRepo));
        }

        /// <summary>
        /// Create a book for author
        /// </summary>
        /// <param name="authorId">The author id to whom book needs to be created</param>
        /// <param name="book">Book Dto</param>
        /// <returns>An ActionResult of type BookDto</returns>
        [HttpPost(Name = nameof(CreateBookForAuthor))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookDto))]
        public async Task<ActionResult<BookDto>> CreateBookForAuthor(long authorId, CreateBookDto book)
        {
            Guard.Against.Null(book, nameof(book));

            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            var command = new CreateBookCommand(book.Title, book.Description, authorId);

            Result<Book> result = await _messages.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var bookToReturn = _mapper.Map<BookDto>(result.Value);
            return CreatedAtRoute(nameof(GetBookForAuthor), new { authorId, bookToReturn.Id }, bookToReturn);
        }
    }
}
