using LibraryWebApi.Models;
using Newtonsoft.Json;

namespace LibraryWebApi.DAL;

public class UsersDatabase : IDatabase
{

    private List<User> _users;

    public UsersDatabase()
    {
        if (!ReadDatabase())
        {
            _users = new List<User>();
        }
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }

    public User GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email.Equals(email));
    }
    
    public void Create(Entity entity)
    {
        _users.Add((User)entity);
    }

    public Entity GetById(Guid id)
    {
        return _users.FirstOrDefault(u => u.Id.Equals(id));
    }

    public void Update(Guid id, Entity updatedEntity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        _users.Remove((User)GetById(id));
    }

    public bool ReadDatabase()
    {
        try
        {
            var data = File.ReadAllText(@".\Storage\users.json");
            _users = JsonConvert.DeserializeObject<List<User>>(data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public bool SaveDatabase()
    {
        var data = JsonConvert.SerializeObject(_users);
        try
        {
            File.WriteAllText(@".\Storage\users.json", data);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}