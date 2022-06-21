using LibraryWebApi.DTO;
using LibraryWebApi.Models;

namespace LibraryWebApi.Helpers.Responses;

public class AuthorsResponse
{
    public List<Author> Authors { get; set; }
}