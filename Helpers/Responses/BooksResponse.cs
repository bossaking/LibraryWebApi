using LibraryWebApi.DTO;

namespace LibraryWebApi.Helpers.Responses;

public class BooksResponse
{
    public List<SingleBook> Books { get; set; }
}