namespace LibraryWebApi.Models;

public class Book : Entity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid AuthorId { get; set; }
}