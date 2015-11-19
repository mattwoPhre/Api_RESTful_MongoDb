using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_API_MVC.Model;

namespace Console_API_MVC.Repository
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();

        Contact GetContact(int id);

        Contact AddContact(Contact item);

        void RemoveContact(int id);

        Contact UpdateContact(int id, Contact item);
    }
}

