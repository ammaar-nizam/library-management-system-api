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
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // Get All Books
        [HttpGet]
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

        // Find a Book by Id
        [HttpGet("{id}")]
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

        // Update a Book by Id
        [HttpPut("{id}")]
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

            return NoContent();
        }

        // Create a Book
        [HttpPost]
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

        // Delete a Book by Id
        [HttpDelete("{id}")]
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

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

    }
}
