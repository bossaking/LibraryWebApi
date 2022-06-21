namespace LibraryWebApi.DTO;

public class NewBookRequest
{
    public string Title { get; set; }
    public Guid AuthorId { get; set; }
}