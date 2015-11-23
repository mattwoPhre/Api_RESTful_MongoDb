using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using Console_API_MVC.Model;
using Console_API_MVC.Repository;
using MongoDB.Bson;

namespace Console_API_MVC.Controller
{
    [FromBody]
    public class ContactsController : ApiController
    {
        private static readonly IContactRepository _contacts = new ContactRepository();

        public IEnumerable<Contact> Get()
        {
            return _contacts.GetAllContacts();
        }

        public Contact Get([FromUri]int id)

            {
            var contact = _contacts.GetContact(id);
            if (contact == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return contact;
        }
        
        public HttpResponseMessage Post([FromBody]Contact value)
        {
            Contact contact = _contacts.AddContact(value);
            return Request.CreateResponse(HttpStatusCode.Created, contact);
        }

        public HttpResponseMessage Put(int id, [FromBody] Contact value)
        {
                Contact contact = _contacts.UpdateContact(id, value);
                return Request.CreateResponse(HttpStatusCode.Created, contact);
        }

        public void Delete(int id)
        {
            _contacts.RemoveContact(id);
        }
    }
}
