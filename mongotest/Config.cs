using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongotest
{
    public class Config
    {
        public string _idconf { get; set; }
        public bool Exist { get; set; }
        public string MongoIP { get; set; }
    }
}
