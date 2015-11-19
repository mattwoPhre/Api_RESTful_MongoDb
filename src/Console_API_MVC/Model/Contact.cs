using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Console_API_MVC.Model
{
    public class Contact
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int ContactID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LastModified { get; set; }
    }
}
