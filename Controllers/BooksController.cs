using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApi.DAL;
using LibraryWebApi.DTO;
using LibraryWebApi.Helpers.Responses;
using LibraryWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private BooksDatabase _booksDatabase;
        private AuthorsDatabase _authorsDatabase;

        public BooksController()
        {
            _booksDatabase = new BooksDatabase();
            _authorsDatabase = new AuthorsDatabase();
        }
        
        [HttpGet]
        [Route("all")]
        public IActionResult GetAllBooks()
        {
            var books = _booksDatabase.GetAllBooks();
            return Ok(new BooksResponse()
            {
                Books = books.Select(b => new SingleBook()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = (Author)_authorsDatabase.GetById(b.AuthorId)
                }).ToList()
            });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("new")]
        public IActionResult CreateNewBook([FromBody] NewBookRequest bookRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Something went wrong... Please try again later."
                    }
                });
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = bookRequest.Title,
                AuthorId = bookRequest.AuthorId
            };

            _booksDatabase.Create(book);
            _booksDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Book has been added successfully!"
                }
            });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        public IActionResult UpdateAuthor([FromQuery] Guid bookId, [FromBody] NewBookRequest bookRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Something went wrong... Please try again later."
                    }
                });
            var book = _booksDatabase.GetById(bookId);
            if (book == null)
            {
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Record does not exists"
                    }
                });
            }

            var newBook = new Book()
            {
                Title = bookRequest.Title,
                AuthorId = bookRequest.AuthorId
            };

            _booksDatabase.Update(bookId, newBook);
            _booksDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Book has been updated successfully!"
                }
            });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteBook([FromQuery] Guid bookId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Something went wrong... Please try again later."
                    }
                });
            var book = _booksDatabase.GetById(bookId);
            if (book == null)
            {
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Record does not exists"
                    }
                });
            }

            _booksDatabase.Delete(bookId);
            _booksDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Book has been deleted successfully!"
                }
            });
        }
    }
}
