using LibraryWebApi.Models;
using Newtonsoft.Json;

namespace LibraryWebApi.DAL;

public class AuthorsDatabase : IDatabase
{

    private List<Author> _authors;

    public AuthorsDatabase()
    {
        if (!ReadDatabase())
        {
            _authors = new List<Author>();
        }
    }

    public List<Author> GetAllAuthors()
    {
        return _authors;
    }

    public void Create(Entity entity)
    {
        _authors.Add((Author)entity);
    }

    public Entity GetById(Guid id)
    {
        return _authors.FirstOrDefault(a => a.Id.Equals(id));
    }

    public void Update(Guid id, Entity updatedEntity)
    {
        var oldValue = (Author)GetById(id);
        oldValue.Name = ((Author)updatedEntity).Name;
        oldValue.Surname = ((Author)updatedEntity).Surname;
    }

    public void Delete(Guid id)
    {
        _authors.Remove((Author)GetById(id));
    }

    public bool ReadDatabase()
    {
        try
        {
            var data = File.ReadAllText(@".\Storage\authors.json");
            _authors = JsonConvert.DeserializeObject<List<Author>>(data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool SaveDatabase()
    {
        var data = JsonConvert.SerializeObject(_authors);
        try
        {
            File.WriteAllText(@".\Storage\authors.json", data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}