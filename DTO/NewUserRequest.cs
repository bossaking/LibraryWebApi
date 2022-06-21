using Microsoft.Build.Framework;

namespace LibraryWebApi.DTO;

public class NewUserRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}