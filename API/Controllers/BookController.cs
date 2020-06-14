using Ardalis.GuardClauses;
using AutoMapper;
using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        /// <param name="book">Create Book Dto</param>
        /// <returns>An ActionResult of type BookDto</returns>
        [HttpPost(Name = nameof(CreateBookForAuthor))]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return CreatedAtRoute(nameof(GetBookForAuthor), 
                new { authorId, bookId = bookToReturn.Id }, bookToReturn);
        }

        /// <summary>
        /// Update a book for author
        /// </summary>
        /// <param name="authorId">The author id to whom book needs to be updated</param>
        /// <param name="bookId">The id of the book to be updated</param>
        /// <param name="book">Book For Update Dto</param>
        /// <returns>An ActionResult of type BookDto or Nothing</returns>
        [HttpPut("{bookId:long:min(1)}", Name = nameof(UpdateBookForAuthor))]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateBookForAuthor(long authorId, long bookId, BookForUpdateDto book)
        {
            Guard.Against.Null(book, nameof(book));

            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            var bookForAuthorFromRepo = await _messages.Dispatch(new GetBookQuery(authorId, bookId));

            // upsert
            if (bookForAuthorFromRepo == null)
            {
                var addCommand = new CreateBookCommand(book.Title, book.Description, authorId);

                Result<Book> addResult = await _messages.Dispatch(addCommand);

                if (addResult.IsFailure)
                {
                    return BadRequest(addResult.Error);
                }

                var bookToReturn = _mapper.Map<BookDto>(addResult.Value);
                return CreatedAtRoute(nameof(GetBookForAuthor), 
                    new { authorId, bookId = bookToReturn.Id }, bookToReturn);
            }

            var updateCommand = new UpdateBookCommand(bookId, book.Title, book.Description, authorId);

            Result<Book> updateResult = await _messages.Dispatch(updateCommand);

            if (updateResult.IsFailure)
            {
                return BadRequest(updateResult.Error);
            }

            return NoContent();
        }

        /// <summary>
        /// Update a book for author
        /// </summary>
        /// <param name="authorId">The author id to whom book needs to be updated</param>
        /// <param name="bookId">The id of the book to be updated</param>
        /// <param name="patchDocument">Book Patch Document</param>
        ///
        /// <remarks>Sample request (this request updates the book's **title**)  
        /// 
        /// PATCH /authors/authorId/books/bookId
        /// [ 
        ///     {
        ///         "op": "replace", 
        ///         "path": "/title", 
        ///         "value": "new title" 
        ///     } 
        /// ] 
        /// </remarks>
        /// <returns>An ActionResult of type BookDto or Nothing</returns>
        [HttpPatch("{bookId:long:min(1)}", Name = nameof(PartiallyUpdateBookForAuthor))]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PartiallyUpdateBookForAuthor(long authorId, long bookId,
            JsonPatchDocument<BookForUpdateDto> patchDocument)
        {
            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            var bookForAuthorFromRepo = await _messages.Dispatch(new GetBookQuery(authorId, bookId));

            if (bookForAuthorFromRepo == null)
            {
                var bookDto = new BookForUpdateDto();
                patchDocument.ApplyTo(bookDto, ModelState);

                if (!TryValidateModel(bookDto))
                {
                    return ValidationProblem(ModelState);
                }

                var addCommand = new CreateBookCommand(bookDto.Title, bookDto.Description, authorId);

                Result<Book> addResult = await _messages.Dispatch(addCommand);

                if (addResult.IsFailure)
                {
                    return BadRequest(addResult.Error);
                }

                var bookToReturn = _mapper.Map<BookDto>(addResult.Value);
                return CreatedAtRoute(nameof(GetBookForAuthor), 
                    new { authorId, bookId = bookToReturn.Id }, bookToReturn);
            }

            var bookToPatch = _mapper.Map<BookForUpdateDto>(bookForAuthorFromRepo);
            // add validation
            patchDocument.ApplyTo(bookToPatch, ModelState);

            if (!TryValidateModel(bookToPatch))
            {
                return ValidationProblem(ModelState);
            }

            var updateCommand = new UpdateBookCommand(bookId, bookToPatch.Title, bookToPatch.Description, authorId);

            Result<Book> updateResult = await _messages.Dispatch(updateCommand);

            if (updateResult.IsFailure)
            {
                return BadRequest(updateResult.Error);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="authorId">The author id of the book to be deleted</param>
        /// <param name="bookId">The id of the book to be deleted</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{bookId:long:min(1)}", Name = nameof(DeleteBookForAuthor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBookForAuthor(long authorId, long bookId)
        {
            var author = await _messages.Dispatch(new GetAuthorQuery(authorId));

            if (author is null)
            {
                return NotFound($"No author with id {authorId} was found.");
            }

            Result result = await _messages.Dispatch(new DeleteBookCommand(bookId));

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
