using System;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Console_API_MVC.Controller;
using Console_API_MVC.Model;
using Console_API_MVC.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NUnit.Framework;

namespace Console_API_MVC_Test
{
    public class RepositoryTest
    {
        public ContactRepository _repo;
        public Contact _contact1;
        public string FirstName;
        public Contact _contact2;
        public IEnumerable<Contact> _listaCont;
        public MongoCollection<Contact> _contacts;
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _database;

        [SetUp]
        public void SetUp()
        {
            string connection = "mongodb://localhost:27017";
            _repo = new ContactRepository();
            _client = new MongoClient(connection);
            _server = _client.GetServer();
            _database = _server.GetDatabase("Prova_Api", WriteConcern.Unacknowledged);
            _contacts = _database.GetCollection<Contact>("ContactTest");

        }

        [Test]
        public void GetAllContacts_ShouldReturnAllProducts()
        {
            var contacts = _repo.GetAllContacts();

            Assert.AreEqual(7, contacts.Count());
        }

        [Test]
        public void GetContact_ShouldReturnOneContact()
        {
            int id = 1;
            var contact = _repo.GetAllContacts();

            var prova = contact.Count(x => x.ContactID == id);

            Assert.AreEqual(1, prova);
        }

        [Test]
        public void Post_ShouldAddContact()
        {
            var tutti = _repo.GetAllContacts().Count();
            var contact = new Contact
            {
                Id = ObjectId.GenerateNewId(),
                ContactID = 123,
                FirstName = "Gionni",
                LastName = "Buonni",
                LastModified = DateTime.Now.Date.ToString()
            };
            _repo.AddContact(contact);
            var tuttiDopoAdd = _repo.GetAllContacts().Count();
            Assert.AreEqual(true, tuttiDopoAdd == tutti + 1);
            _repo.RemoveContact(contact.ContactID);
        }

        [Test]
        public void Delete_ShouldRemoveContact()
        {
            var quanti = _repo.GetAllContacts().Count();
            var contact = new Contact
            {
                Id = ObjectId.GenerateNewId(),
                ContactID = 123,
                FirstName = "Gionni",
                LastName = "Buonni",
                LastModified = DateTime.Now.Date.ToString()
            };
            _repo.AddContact(contact);
            _repo.RemoveContact(contact.ContactID);
            var tuttiDopoRemove = _repo.GetAllContacts().Count();

            Assert.AreEqual(true, tuttiDopoRemove == quanti);

        }

        [Test]
        public void Update_ShouldEditContact()
        {
            int id = 2;
            string nome = "Johnny";
            string data_modificata = "Today";
            var contact = _repo.GetContact(id);
            IMongoQuery query = Query.EQ("ContactID", id);


            using (TransactionScope ts = new TransactionScope())
            {
                IMongoUpdate update = Update
                    .Set("ContactID", id)
                    .Set("FirstName", nome)
                    .Set("LastName", contact.LastName)
                    .Set("LastModified", data_modificata);

                Contact daModificare = new Contact
                {
                    ContactID = contact.ContactID,
                    FirstName = nome,
                    LastName = contact.LastName,
                    LastModified = data_modificata
                };
                _contacts.Update(query, update);
                _repo.UpdateContact(id, daModificare);

                Assert.AreEqual(true, daModificare.FirstName == nome);
                scope.Dispose();

            }
        }


    }
}

