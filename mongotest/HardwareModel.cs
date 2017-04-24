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
using System.IO;
using System.Xml.Serialization;

namespace mongotest
{

    class HardwareModel
    {
        public string _id { get; set; }
        public string ComputerName { get; set; }
        public string OsVersion { get; set; }
        public string ServicePack { get; set; }
        public string UserName { get; set; }
        public string Is64 { get; set; }
        public int TickCount { get; set; }
        public string UserDomain { get; set; }
        public CPU _cpu { get; set; }
        public GPU _gpu { get; set; }
        public Memory _memory { get; set; }
        public Motherboard _mb { get; set; }
        public HDD _hdd { get; set; }


        public void Start()
        {
            XmlSerializer sr = new XmlSerializer(typeof(Config));
            TextReader tw = new StreamReader("C:\\\\ATHService\\config.xml");
            
            Config conf = (Config)sr.Deserialize(tw);
            tw.Close();

            if (conf.Exist == true)
            {
                var hw = new HardwareModel();
                hw.ReplaceDocument(hw, conf._idconf);
            }
            else
            {
                var hw = new HardwareModel();
                hw.CreateFirst(hw);
            }

            System.Threading.Thread.Sleep(conf.Refresh);
            Start();
            
        }

        public void Stop()
        {

        }

        public void CreateFirst(HardwareModel hw)
        {
            Config conf = new Config();

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

            conf._idconf = hw._id;
            conf.Exist = true;
            XmlSerializer sr = new XmlSerializer(typeof(Config));
            TextWriter tw = new StreamWriter("C:\\\\ATHService\\config.xml");
            sr.Serialize(tw, conf);
            tw.Close();
            

        }


        async public void ReplaceDocument(HardwareModel hw, string tmpid)
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
            hw._id = tmpid;
            var filter = Builders<HardwareModel>.Filter.Eq(s => s._id, tmpid);
            await coll.DeleteOneAsync(filter);
            await coll.InsertOneAsync(hw);
            
        }


        public HardwareModel()
        {
            var OpenHW = new Computer()
            {
                CPUEnabled = true,
                FanControllerEnabled = true,
                GPUEnabled = true,
                HDDEnabled = true,
                MainboardEnabled = true,
                RAMEnabled = true
            };
            OpenHW.ToCode();
            OpenHW.Open();

            var tmp = OpenHW.GetReport();
            _id = Environment.MachineName.ToString() + Environment.TickCount.ToString();
            ComputerName = Environment.MachineName;
            ServicePack = Environment.OSVersion.ServicePack;
            UserDomain = Environment.UserDomainName;
            UserName = Environment.UserName;
            if (Environment.Is64BitOperatingSystem)
            {
                Is64 = "x64";
            }
            else
            {
                Is64 = "x86";
            }
            //Is64 = Environment.Is64BitOperatingSystem.ToString();
            TickCount = Environment.TickCount / 1000 / 60;
            //OsVersion = Environment.Version.ToString();
            var OStrim = Environment.OSVersion.VersionString.Remove(0, 21);
            OStrim = OStrim.Remove(4, (OStrim.Count() - 4));
            if (OStrim == "10.0")
            {
                OsVersion = "Windows 10";
            }
            else if (OStrim == "6.3.")
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


            
            _gpu = new GPU()
            {
                GPUClocks = new List<string>(),
                GPULoads = new List<string>(),
                GPUData = new List<string>()
            };
            _hdd = new HDD();
            _mb = new Motherboard();
            _memory = new Memory();
            _cpu = new CPU()
            {
                CPUCoreClock = new List<double>(),
                CPUCoreLoad = new List<double>(),
                CPUCoreTemp = new List<double>(),
                CPUPowers = new List<string>(),
                Other = new List<string>()
            };
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
                            round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
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
