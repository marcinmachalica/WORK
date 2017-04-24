using MongoDB.Bson;
using MongoDB.Driver;
using mongotest;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Admin.Models
{
    public class AdminHardwareModel
    {
        public string _id { get; set; }
        public string ComputerName { get; set; }
        public string OsVersion { get; set; }
        public string ServicePack { get; set; }
        public string UserName { get; set; }
        public bool Is64 { get; set; }
        public int TickCount { get; set; }
        public string UserDomain { get; set; }
        public CPU _cpu { get; set; }
        public GPU _gpu { get; set; }
        public Memory _memory { get; set; }
        public Motherboard _mb { get; set; }
        public HDD _hdd { get; set; }

        async public Task<List<AdminHardwareModel>> ReadMongoBase()
        {
            

            var _client = new MongoClient();
            var _db = _client.GetDatabase("ComputersStore");
            var coll = _db.GetCollection<AdminHardwareModel>("Computer");

            
            return await coll.Find(_ => true).ToListAsync();

            
        }
    }
}