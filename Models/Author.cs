namespace LibraryWebApi.Models;

public class Author : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}