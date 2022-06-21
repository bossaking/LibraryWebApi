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
    [Route("authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorsDatabase _authorsDatabase;

        public AuthorsController()
        {
            _authorsDatabase = new AuthorsDatabase();
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllAuthors()
        {
            var authors = _authorsDatabase.GetAllAuthors();
            return Ok(new AuthorsResponse()
            {
                Authors = authors
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("new")]
        public IActionResult CreateNewAuthor([FromBody] NewAuthorRequest authorRequest)
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
            var author = new Author()
            {
                Id = Guid.NewGuid(),
                Name = authorRequest.Firstname,
                Surname = authorRequest.Lastname
            };

            _authorsDatabase.Create(author);
            _authorsDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Author has been added successfully!"
                }
            });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        public IActionResult UpdateAuthor([FromQuery] Guid authorId, [FromBody] NewAuthorRequest authorRequest)
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
            var author = _authorsDatabase.GetById(authorId);
            if (author == null)
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

            var newAuthor = new Author()
            {
                Name = authorRequest.Firstname,
                Surname = authorRequest.Lastname
            };

            _authorsDatabase.Update(authorId, newAuthor);
            _authorsDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Author has been updated successfully!"
                }
            });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteAuthor([FromQuery] Guid authorId)
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
            var author = _authorsDatabase.GetById(authorId);
            if (author == null)
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

            _authorsDatabase.Delete(authorId);
            _authorsDatabase.SaveDatabase();

            return Ok(new ActionResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Author has been deleted successfully!"
                }
            });
        }
    }
}