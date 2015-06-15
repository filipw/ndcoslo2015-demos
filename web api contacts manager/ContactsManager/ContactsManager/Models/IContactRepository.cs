using System.Linq;

namespace ContactsManager.Models
{
    public interface IContactRepository
    {
        IQueryable<Contact> GetAll();

        Contact Get(int id);

        void Add(Contact contact);

        void Update(Contact updatedContact);

        void Delete(int id);
    }
}