using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.ViewModels
{
    public class IndexViewModel
    {
        public string ComputerName { get; set; }
        public string OsVersion { get; set; }
        public string UserName { get; set; }
        public string Is64 { get; set; }
        public int TickCount { get; set; }
        public string UserDomain { get; set; }
        public string CpuName { get; set; }
        public string GpuName { get; set; }
        public string RAMsize { get; set; }
        public string Mbname { get; set; }
        public string HDDName { get; set; }
    }
}