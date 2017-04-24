using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OpenHardwareMonitor.Hardware;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mongotest;


namespace mongotest
{
    class HardwareModel
    {
        public ObjectId _id { get; set; }
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
        



        public void createFirst(HardwareModel hw)
        {


            var _client = new MongoClient();
            var _db = _client.GetDatabase("ComputersStore");
            var coll = _db.GetCollection<HardwareModel>("Computer");

            hw.ToBsonDocument();

            if (!BsonClassMap.IsClassMapRegistered(typeof(HardwareModel)))
            {
                BsonClassMap.RegisterClassMap<HardwareModel>(
                    cm => { cm.AutoMap(); });
            }
            var filter = Builders<HardwareModel>.Filter.Eq("_id", hw._id);
            coll.InsertOne(hw);

        }


        async public void replaceDocument(HardwareModel hw,ObjectId tmpid)
        {
            var _client = new MongoClient();
            var _db = _client.GetDatabase("ComputersStore");
            var coll = _db.GetCollection<HardwareModel>("Computer");

            hw.ToBsonDocument();

            if (!BsonClassMap.IsClassMapRegistered(typeof(HardwareModel)))
            {
                BsonClassMap.RegisterClassMap<HardwareModel>(
                    cm => { cm.AutoMap(); });
            }
            //Computer c1 = new Computer() {name="comp1",model="dell" };
            //var filter = Builders<HardwareModel>.Filter.Eq("_id", hw._id);
            hw._id = tmpid;
             
            // passed in 
            var filter = Builders<HardwareModel>.Filter.Eq(s => s._id,tmpid);
            await coll.DeleteOneAsync(filter);
            await coll.InsertOneAsync(hw);
            //coll.UpdateOne(filter, hw);
            
        }
        

        public HardwareModel()
        {
            var OpenHW = new Computer();

            
            OpenHW.CPUEnabled = true;
            OpenHW.FanControllerEnabled = true;
            OpenHW.GPUEnabled = true;
            OpenHW.HDDEnabled = true;
            OpenHW.MainboardEnabled = true;
            OpenHW.RAMEnabled = true;
            OpenHW.ToCode();
            OpenHW.Open();

            var tmp = OpenHW.GetReport();

            ComputerName = Environment.MachineName;
            UserDomain = Environment.UserDomainName;
            UserName = Environment.UserName;
            Is64 = Environment.Is64BitOperatingSystem;
            TickCount = Environment.TickCount / 1000 / 60;
            //OsVersion = Environment.Version.ToString();
            var OStrim = Environment.OSVersion.VersionString.Remove(0, 21);
            OStrim=OStrim.Remove(4, (OStrim.Count() - 4));
            if (OStrim=="10.0")
            {
                OsVersion = "Windows 10";
            }else if(OStrim=="6.3.")
            {
                OsVersion = "Windows 8.1";
            }
            else if (OStrim == "6.2.")
            {
                OsVersion = "Windows 8";
            }
            else if (OStrim == "6.1.")
            {
                OsVersion = "Windows 7";
            }
            else if (OStrim == "6.0.")
            {
                OsVersion = "Windows Vista";
            }
            else if (OStrim == "5.2.")
            {
                OsVersion = "Windows XP Professional x64";
            }
            else if (OStrim == "5.1.")
            {
                OsVersion = "Windows XP";
            }


            _cpu = new CPU();
            _gpu = new GPU();
            _gpu.GPUClocks = new List<string>();
            _gpu.GPULoads = new List<string>();
            _gpu.GPUData = new List<string>();
            _hdd = new HDD();
            _mb = new Motherboard();
            _memory = new Memory();
            _cpu.CPUCoreClock = new List<double>();
            _cpu.CPUCoreLoad = new List<double>();
            _cpu.CPUCoreTemp = new List<double>();
            _cpu.CPUPowers = new List<string>();
            _cpu.Other = new List<string>();
            _memory.UsedMemory = new List<double>();
            _hdd.HDName = new List<string>();
            _hdd.HDTemp = new List<double>();
            _hdd.HDUsedSpace = new List<double>();
            double round;
            foreach (var item in OpenHW.Hardware)
            {
                if (item.HardwareType == HardwareType.CPU)
                {
                    item.Update();
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                    }
                    foreach (var sensor in item.Sensors)
                    {
                        _cpu.CPUName = item.Name;
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()));
                            _cpu.CPUCoreClock.Add(round);
                            //_cpu.BusSpeed += string.Format("{0} {1}\n", sensor.Name, sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                            _cpu.CPUCoreTemp.Add(round);
                        }

                        if (sensor.SensorType == SensorType.Load)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                            _cpu.CPUCoreLoad.Add(round);
                        }
                        if (sensor.SensorType == SensorType.Power)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                            _cpu.CPUPowers.Add(round.ToString());

                        }
                        if (sensor.SensorType == SensorType.Level)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Control)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Factor)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Fan)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Flow)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Voltage)
                        {
                            _cpu.Other.Add(sensor.Value.ToString());
                        }
                    }
                    _cpu.BusSpeed = _cpu.CPUCoreClock.Last();
                    _cpu.CPUCoreClock.RemoveAt(_cpu.CPUCoreClock.Count - 1);

                    _cpu.CPUTotalLoad = _cpu.CPUCoreLoad.Last();
                    _cpu.CPUCoreLoad.RemoveAt(_cpu.CPUCoreLoad.Count - 1);

                    _cpu.CPUPackageTemp = _cpu.CPUCoreTemp.Last();
                    _cpu.CPUCoreTemp.RemoveAt(_cpu.CPUCoreTemp.Count - 1);



                }
                if (item.HardwareType == HardwareType.GpuNvidia)
                {
                    item.Update();
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                    }
                    foreach (var sensor in item.Sensors)
                    {
                        _gpu.GPUName = item.Name;
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            _gpu.GPUClocks.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            _gpu.GPULoads.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            _gpu.GPUCoreTemp = sensor.Value.ToString();
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            _gpu.GPUData.Add(sensor.Value.ToString());
                        }
                    }

                }
                if (item.HardwareType == HardwareType.GpuAti)
                {
                    item.Update();
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                    }
                    foreach (var sensor in item.Sensors)
                    {
                        _gpu.GPUName = item.Name;
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            _gpu.GPUClocks.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            _gpu.GPULoads.Add(sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            _gpu.GPUCoreTemp = sensor.Value.ToString();
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            _gpu.GPUData.Add(sensor.Value.ToString());
                        }
                    }

                }
                if (item.HardwareType == HardwareType.RAM)
                {
                    item.Update();
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                        _memory.Name = subHardware.Name;
                    }
                    foreach (var sensor in item.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()));
                            _memory.MemoryLoad = round;
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()),1);
                            _memory.UsedMemory.Add(round);
                        }
                        
                    }
                }
                if (item.HardwareType == HardwareType.HDD)
                {
                    item.Update();
                    _hdd.HDName.Add(item.Name);
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                       
                    }
                    foreach (var sensor in item.Sensors)
                    {
                        
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()));
                            _hdd.HDTemp.Add(round);
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                            _hdd.HDUsedSpace.Add(round);
                        }

                    }
                }
                if (item.HardwareType == HardwareType.Mainboard)
                {
                    item.Update();
                    foreach (var subHardware in item.SubHardware)
                    {
                        subHardware.Update();
                        _mb.MBName = subHardware.Name;
                    }
                    
                }
                

            }

        }


    }
}
