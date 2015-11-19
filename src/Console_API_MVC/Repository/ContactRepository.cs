using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Console_API_MVC.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Console_API_MVC.Repository
{
    public class ContactRepository : IContactRepository
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _database;
        MongoCollection<Contact> _contacts;

        public ContactRepository()
            : this("")
        {
        }

        public ContactRepository(string connection)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = "mongodb://localhost:27017";
            }

            _client = new MongoClient(connection);
            _server = _client.GetServer();
            _database = _server.GetDatabase("Prova_Api", WriteConcern.Unacknowledged);
            _contacts = _database.GetCollection<Contact>("Contact");

        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _contacts.FindAll();
        }

        public Contact GetContact(int id)
        {
            IMongoQuery query = Query.EQ("ContactID", id);
            return _contacts.Find(query).FirstOrDefault();
        }

        public Contact AddContact(Contact item)
        {
            Contact contact = new Contact
            {
                Id = ObjectId.GenerateNewId(),
                ContactID = item.ContactID,
                FirstName = item.FirstName,
                LastName = item.LastName,
                LastModified = DateTime.Now.Date.ToString()

            };
            _contacts.Insert(contact);
            return item;
        }

        public void RemoveContact(int id)
        {
            IMongoQuery query = Query.EQ("ContactID", id);
            _contacts.Remove(query);
        }

        public Contact UpdateContact(int id, Contact item)
        {
            IMongoQuery query = Query.EQ("ContactID", id);
            item.LastModified = "Today";
            IMongoUpdate update = Update
                .Set("ContactID", item.ContactID)
                .Set("FirstName", item.FirstName)
                .Set("LastName", item.LastName);

            _contacts.Update(query, update);
            return item;
        }
    }
}
