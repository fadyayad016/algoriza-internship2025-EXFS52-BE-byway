using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.DTOs
{
    public class Auth
    {

        public class RegisterDto
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string Username { get; set; }

            public string FirstName { get; set; }

    
            public string LastName { get; set; }
        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }


        public class UserDto
        {
            public string Username { get; set; }
            public string Token { get; set; }
            public string Email { get; set; }
        }
    }
}
