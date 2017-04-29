using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Admin.Models;

namespace Admin.ViewModels
{
    public class DetailsViewModel
    {
        public string Id { get; set; }
        public string ComputerName { get; set; }
        public string UserName { get; set; }
        public string OsVersion { get; set; }
        public string Is64 { get; set; }
        public string ServicePack { get; set; }
        public string UserDomain { get; set; }
        public string LastUpdate { get; set; }
        public int TickCount { get; set; }
        public CPU Cpu { get; set; }
        public GPU Gpu { get; set; }
        //public Config config { get; set; }
        public Memory Memory { get; set; }
        public Motherboard Mb { get; set; }
        public HDD Hdd { get; set; }

        public DetailsViewModel(AdminHardwareModel adm)//,Config conf)
        {

            Cpu = new CPU();
            Gpu = new GPU();
            Memory = new Memory();
            Mb = new Motherboard();
            Hdd = new HDD();

            Id = adm._id;
            ComputerName = adm.ComputerName;
            UserName = adm.UserName;
            OsVersion = adm.OsVersion;
            Is64 = adm.Is64;
            ServicePack = adm.ServicePack;
            UserDomain = adm.UserDomain;
            LastUpdate = adm.LastUpdate;
            TickCount = adm.TickCount;
            Cpu.BusSpeed = adm._cpu.BusSpeed;
            Cpu.CPUCoreClock = adm._cpu.CPUCoreClock;
            Cpu.CPUCoreLoad = adm._cpu.CPUCoreLoad;
            Cpu.CPUCoresPow = adm._cpu.CPUCoresPow;
            Cpu.CPUCoreTemp = adm._cpu.CPUCoreTemp;
            Cpu.CPUGraphicsPow = adm._cpu.CPUGraphicsPow;
            Cpu.CPUName = adm._cpu.CPUName;
            Cpu.CPUPackagePow = adm._cpu.CPUPackagePow;
            Cpu.CPUPackageTemp = adm._cpu.CPUPackageTemp;
            Cpu.CPUPowers = adm._cpu.CPUPowers;
            Cpu.CPUTotalLoad = adm._cpu.CPUTotalLoad;
            Cpu.Other = adm._cpu.Other;
            Gpu.GPUClocks = adm._gpu.GPUClocks;
            Gpu.GPUCore = adm._gpu.GPUCore;
            Gpu.GPUCoreLoad = adm._gpu.GPUCoreLoad;
            Gpu.GPUCoreTemp = adm._gpu.GPUCoreTemp;
            Gpu.GPUData = adm._gpu.GPUData;
            Gpu.GPULoads = adm._gpu.GPULoads;
            Gpu.GPUMemory = adm._gpu.GPUMemory;
            Gpu.GPUMemoryContLoad = adm._gpu.GPUMemoryContLoad;
            Gpu.GPUMemoryFree = adm._gpu.GPUMemoryFree;
            Gpu.GPUMemoryLoad = adm._gpu.GPUMemoryLoad;
            Gpu.GPUMemoryTotal = adm._gpu.GPUMemoryTotal;
            Gpu.GPUMemoryUsed = adm._gpu.GPUMemoryUsed;
            Gpu.GPUName = adm._gpu.GPUName;
            Gpu.GPUShader = adm._gpu.GPUShader;
            Gpu.GPUVEngineLoad = adm._gpu.GPUVEngineLoad;
            Hdd.HDName = adm._hdd.HDName;
            Hdd.HDTemp = adm._hdd.HDTemp;
            Hdd.HDUsedSpace = adm._hdd.HDUsedSpace;
            Mb.MBName = adm._mb.MBName;
            Memory.Name = adm._memory.Name;
            Memory.MemoryLoad = adm._memory.MemoryLoad;
            Memory.UsedMemory = adm._memory.UsedMemory;
            //config = conf;



        }

    }
}