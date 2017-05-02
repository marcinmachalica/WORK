using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace mongotest
{
    public class Config
    {
        public string _idconf { get; set; }
        public bool Exist { get; set; }
        public string MongoIP { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public int Refresh { get; set; }

        public IMongoCollection<Model> Connect()
        {
            var conf = new Config();
            XmlSerializer sr = new XmlSerializer(typeof(Config));
            TextReader tw = new StreamReader("C:\\\\ATHService\\config.xml");

            conf = (Config)sr.Deserialize(tw);
            tw.Close();
            var _client = new MongoClient("mongodb://" + conf.MongoIP + ":27017");
            var _db = _client.GetDatabase(conf.Database);
            var coll = _db.GetCollection<Model>(conf.Collection);
            return coll;
        }
    }
}
