using LibraryWebApi.Models;
using Newtonsoft.Json;

namespace LibraryWebApi.DAL;

public class BooksDatabase : IDatabase
{

    private List<Book> _books;

    public BooksDatabase()
    {
        if (!ReadDatabase())
        {
            _books = new List<Book>();
        }
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }

    public void Create(Entity entity)
    {
        _books.Add((Book)entity);
    }

    public Entity GetById(Guid id)
    {
        return _books.FirstOrDefault(a => a.Id.Equals(id));
    }

    public void Update(Guid id, Entity updatedEntity)
    {
        var oldValue = (Book)GetById(id);
        oldValue.Title = ((Book)updatedEntity).Title;
        oldValue.AuthorId = ((Book)updatedEntity).AuthorId;
    }

    public void Delete(Guid id)
    {
        _books.Remove((Book)GetById(id));
    }

    public bool ReadDatabase()
    {
        try
        {
            var data = File.ReadAllText(@".\Storage\books.json");
            _books = JsonConvert.DeserializeObject<List<Book>>(data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool SaveDatabase()
    {
        var data = JsonConvert.SerializeObject(_books);
        try
        {
            File.WriteAllText(@".\Storage\books.json", data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}