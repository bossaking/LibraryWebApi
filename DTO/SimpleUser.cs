using LibraryWebApi.Enums;

namespace LibraryWebApi.DTO;

public class SimpleUser
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
}