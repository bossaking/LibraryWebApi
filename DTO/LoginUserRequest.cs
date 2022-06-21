using Microsoft.Build.Framework;

namespace LibraryWebApi.DTO;

public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}