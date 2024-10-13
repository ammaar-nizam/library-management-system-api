using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystemAPI.Data;
using LibraryManagementSystemAPI.Models;

namespace LibraryManagementSystemAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL BOOKS
        /// <summary>
        /// Lists All Books.
        /// </summary>
        /// <returns>Returns a list of all the books</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/books
        ///
        /// </remarks>
        /// <response code="200">Returns a list of books</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {   // Return the list of books
                return await _context.Books.ToListAsync();  
            }
            // For unknown exceptions
            catch (Exception)   
            {      
                return StatusCode(500, "Internal Server Error: Something went wrong.");
            }
        }

        // FIND A BOOK BY ID
        /// <summary>
        /// Finds a Book by Id.
        /// </summary>
        /// /// <param name="id">The Id of the book to find (from the URL).</param>
        /// <returns>Returns a book for the given Id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/books/{id}
        ///
        /// </remarks>
        /// <response code="200">Returns a book</response>
        /// <response code="400">If there is a bad request or validation errors</response>
        /// <response code="404">If the book is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Book>> GetBook(long id)
        {   // Check if the user input is a positive number
            if (id <= 0)    
            {
                return BadRequest("Validation Error: Id must be a positive number.");
            }
            try {
                var book = await _context.Books.FindAsync(id);
                
                if (book == null)
                {   // If the book is not available
                    return NotFound($"Resource Not Found: A book with id {id} does not exist.");
                }
                // If the book is available. return the book
                return book;    
            }
            // For unknown exceptions
            catch (Exception){    
                return StatusCode(500, "Internal Server Error: Something went wrong.");
            }
        }

        // CREATE A BOOK
        /// <summary>
        /// Creates a Book.
        /// </summary>
        /// <param name="book">The book object containing the book data (from the request body).</param>
        /// <returns>A newly created book</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/books
        ///     {
        ///        "title": "Sample Book",
        ///        "author": "Sample Author",
        ///        "description": "Sample Description",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created book</response>
        /// <response code="400">If the book is null</response>
        /// <response code="409">If there is a conflict in the database</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {   // Check if a book with the same Id already exists
            if (book.Id > 0)
            {
                var existingBookId = await _context.Books.FindAsync(book.Id);
                if (existingBookId != null)
                {
                    return Conflict("Conflict on the Server: A book with this Id already exists.");
                }
            }
            // Check if the book already exists with the same title
            var existingBookTitle = await _context.Books
                .Where(b => b.Title.ToLower() == book.Title.ToLower() || b.Id == book.Id)
                .FirstOrDefaultAsync();

            if (existingBookTitle != null)
            {
                return Conflict("Conflict on the Server: A book with this title already exists.");
            }

            try
            {   // Save the book to database
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                // Return the created book
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            // For unknown exceptions
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error: Something went wrong.");
            }
        }

        // UPDATE A BOOK BY ID
        /// <summary>
        /// Updates a Book.
        /// </summary>
        /// <param name="id">The Id of the book to update (from the URL).</param>
        /// <param name="book">The book object containing the updated data (from the request body).</param>
        /// <returns>An updated book</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/books/{id}
        ///     {
        ///        "title": "Sample Update Book",
        ///        "author": "Sample Update Author",
        ///        "description": "Sample Update Description",
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Returns no content</response>
        /// <response code="400">If there is a validation error</response>
        /// <response code="404">If the book is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutBook(long id, Book book)
        {   // Check if the user input is a positive number
            if (id <= 0)    
            {
                return BadRequest("Validation Error: Id must be a positive number.");
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();     
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {   // If the book is not available to update
                if (!BookExists(id))
                {   // If the book is not available
                    return NotFound($"Resource Not Found: A book with id {id} does not exist to update.");
                }
                else
                {
                    return StatusCode(500, "Database Error: An issue occurred while updating the book.");
                }
            }
            // For unknown exceptions
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error: Something went wrong.");
            }
        }

        // DELETE A BOOK BY ID
        /// <summary>
        /// Deletes a Book.
        /// </summary>
        /// <param name="id">The Id of the book to delete (from the URL).</param>
        /// <returns>Returns nothing</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/books/{id}
        ///
        /// </remarks>
        /// <response code="204">Returns no content</response>
        /// <response code="404">If the book is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(long id)
        {   // Check if the user input is a positive number
            if (id <= 0)    
            {
                return BadRequest("Validation Error: Id must be a positive number.");
            }

            try
            {
                var book = await _context.Books.FindAsync(id);

                if (book == null)  
                {   // If the book is not available
                    return NotFound($"Resource Not Found: A book with id {id} does not exist to delete.");
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Database Error: Unable to delete the book.");
            }
            // For unknown exceptions
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error: Something went wrong.");
            }

        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

    }
}
