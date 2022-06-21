using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryWebApi.DAL;
using LibraryWebApi.DTO;
using LibraryWebApi.Helpers.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("users/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersDatabase _usersDatabase;

        public UsersController()
        {
            _usersDatabase = new UsersDatabase();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("all")]
        public IActionResult GetAllUsers()
        {
            var users = _usersDatabase.GetAllUsers();
            return Ok(new UsersResponse()
            {
                Users = users.Select(u => new SimpleUser()
                {
                    Id = u.Id,
                    Firstname = u.FirstName,
                    Lastname = u.LastName,
                    Email = u.Email,
                    Role = u.Role
                }).ToList()
            });
        }
    }
}
