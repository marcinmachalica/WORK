using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
    public class Model
    {
        public string _id { get; set; }
        public HardwareModel Hardware { get; set; }
        public SoftwareModel Software { get; set; }

        public void Start()
        {
            if (File.Exists("C:\\\\ATHService\\config.xml"))
            {
                XmlSerializer sr = new XmlSerializer(typeof(Config));

                TextReader tw = new StreamReader("C:\\\\ATHService\\config.xml");
                Config conf = (Config)sr.Deserialize(tw);
                tw.Close();
                var model = new Model()
                {
                Hardware = new HardwareModel(),
                    Software = new SoftwareModel()
                };
                model.ReplaceDocument(model, conf._idconf);
                System.Threading.Thread.Sleep(conf.Refresh);
                Start();
            }
            else
            {
                Model model = new Model()
                {
                    _id = Environment.MachineName.ToString() + Environment.TickCount.ToString() + Environment.UserDomainName,
                    Hardware = new HardwareModel(),
                    Software = new SoftwareModel()
                };
                model.CreateFirst(model);
                Start();
            }



        }

        public void Stop()
        {

        }

        public void CreateFirst(Model model)
        {
            Config config = new Config()
            {
                _idconf = model._id,
                Exist = true,
                MongoIP = "192.168.1.54",
                Database = "ComputersStore",
                Collection = "Computer",
                Refresh = 10000
            };
            XmlSerializer sr = new XmlSerializer(typeof(Config));
            Directory.CreateDirectory("C:\\\\ATHService");
            TextWriter tw = new StreamWriter("C:\\\\ATHService\\config.xml");
            sr.Serialize(tw, config);
            tw.Close();

            var coll = config.Connect();

            model.ToBsonDocument();

            if (!BsonClassMap.IsClassMapRegistered(typeof(Model)))
            {
                BsonClassMap.RegisterClassMap<Model>(
                    cm => { cm.AutoMap(); });
            }
            var filter = Builders<Model>.Filter.Eq("_id", model._id);
            try
            {
                coll.InsertOne(model);
            }
            catch { Start(); };
        }


        async public void ReplaceDocument(Model model, string tmpid)
        {
            Config conf = new Config();
            var coll = conf.Connect();
            model.ToBsonDocument();
            if (!BsonClassMap.IsClassMapRegistered(typeof(Model)))
            {
                BsonClassMap.RegisterClassMap<Model>(
                    cm => { cm.AutoMap(); });
            }
            model._id = tmpid;
            var filter = Builders<Model>.Filter.Eq(s => s._id, tmpid);
            await coll.DeleteOneAsync(filter);
            try
            {
                await coll.InsertOneAsync(model);
            }
            catch { Start(); };

        }
    }
}
