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

    public class HardwareModel
    {
        public string ComputerName { get; set; }
        public string OsVersion { get; set; }
        public string ServicePack { get; set; }
        public string UserName { get; set; }
        public string Is64 { get; set; }
        public int TickCount { get; set; }
        public string LastUpdate { get; set; }
        public string UserDomain { get; set; }
        public CPU _cpu { get; set; }
        public GPU _gpu { get; set; }
        public Memory _memory { get; set; }
        public Motherboard _mb { get; set; }
        public HDD _hdd { get; set; }
        


        




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
            
            ComputerName = Environment.MachineName;
            ServicePack = Environment.OSVersion.ServicePack;
            UserDomain = Environment.UserDomainName;
            UserName = Environment.UserName;
            LastUpdate = System.DateTime.Now.ToString();
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
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()));
                                _cpu.CPUCoreClock.Add(round);
                            }
                            //_cpu.BusSpeed += string.Format("{0} {1}\n", sensor.Name, sensor.Value.ToString());
                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                                _cpu.CPUCoreTemp.Add(round);
                            }
                        }

                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                                _cpu.CPUCoreLoad.Add(round);
                            }
                        }
                        if (sensor.SensorType == SensorType.Power)
                        {
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                                _cpu.CPUPowers.Add(round.ToString());
                            }

                        }
                        if (sensor.SensorType == SensorType.Level)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Control)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Factor)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Fan)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Flow)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Voltage)
                        {
                            if (sensor.Value != null)
                            {
                                _cpu.Other.Add(sensor.Value.ToString());
                            }
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
                            if (sensor.Value != null)
                            {
                                _gpu.GPUClocks.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPULoads.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPUCoreTemp = sensor.Value.ToString();
                            }
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPUData.Add(sensor.Value.ToString());
                            }
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
                            if (sensor.Value != null)
                            {
                                _gpu.GPUClocks.Add(sensor.Value.ToString());
                            }
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPULoads.Add(sensor.Value.ToString());
                            }
                        }

                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPUCoreTemp = sensor.Value.ToString();
                            }
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Value != null)
                            {
                                _gpu.GPUData.Add(sensor.Value.ToString());
                            }
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
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()));
                                _memory.MemoryLoad = round;
                            }
                        }
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                                _memory.UsedMemory.Add(round);
                            }
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
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()));
                                _hdd.HDTemp.Add(round);
                            }
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Value != null)
                            {
                                round = Math.Round(Double.Parse(sensor.Value.ToString()), 1);
                                _hdd.HDUsedSpace.Add(round);
                            }
                        }

                    }
                }
                if (item.HardwareType == HardwareType.Mainboard)
                {
                    item.Update();
                    _mb.MBName = item.Name;
                    //foreach (var subHardware in item.SubHardware)
                    //{
                    //    subHardware.Update();
                    //    _mb.MBName = subHardware.Name;

                    //}

                }


            }

        }


    }
}
