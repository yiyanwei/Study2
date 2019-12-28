using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Models;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {

        }

        [HttpGet("{userName}")]
        public bool IsValid(string userName)
        {
            if(!string.IsNullOrEmpty(userName))
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public IList<User> GetUserListAsync(User user)
        {
            List<User> users = new List<User>();
            return users;
        }
    }
}