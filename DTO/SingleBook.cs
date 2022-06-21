using LibraryWebApi.Models;

namespace LibraryWebApi.DTO;

public class SingleBook
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
}