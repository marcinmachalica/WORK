using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
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
                RemoveSoft(conf._idconf);
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

        async public void RemoveSoft(string id)
        {
            try
            {
                Config conf = new Config();
                var coll = conf.Connect();
                Model model = await coll.Find(_ => _._id == id).SingleAsync();
                foreach (var item in model.Software.Programs)
                {
                    if (item.Remove == true)
                    {
                        using (PowerShell PowerShellInstance = PowerShell.Create())
                        {
                            PowerShellInstance.AddScript("$app=Get-WmiObject -Class Win32_Product | where-object {$_.identifyingNumber -match "+item.IdentifyingNumber +"}");
                            PowerShellInstance.AddScript("$app.uninstall()");
                            PowerShellInstance.Invoke();
                            System.Threading.Thread.Sleep(30000);
                        }
                        
                    }
                }
                foreach (var item in model.Software.Processes)
                {
                    if (item.Remove == true)
                    {
                        using (PowerShell PowerShellInstance = PowerShell.Create())
                        {
                            PowerShellInstance.AddScript("stop-process "+item.ProcesId);
                            PowerShellInstance.Invoke();
                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                }
                using (PowerShell PowerShellInstance = PowerShell.Create())
                {
                    PowerShellInstance.AddScript("Get-WmiObject -Class Win32_Product |Sort-Object Name | select IdentifyingNumber,Name,Vendor,Version,Caption,Description | export-csv C:\\\\ATHService\\Programs.csv");
                    PowerShellInstance.AddScript("get-process | Sort-Object ProcessName | select ProcessName,Id | export-csv C:\\\\ATHService\\Processes.csv");
                    PowerShellInstance.Invoke();
                    System.Threading.Thread.Sleep(20000);
                }
                
            }
            catch { Start(); };
        }

    }
}
