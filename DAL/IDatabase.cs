using LibraryWebApi.Models;

namespace LibraryWebApi.DAL;

public interface IDatabase
{
    bool ReadDatabase();
    bool SaveDatabase();
    void Create(Entity entity);
    Entity GetById(Guid id);
    void Update(Guid id, Entity updatedEntity);
    void Delete(Guid id);
}