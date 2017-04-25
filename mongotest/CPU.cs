using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongotest
{
    public class CPU
    {
        public string CPUName { get; set; }
        public double BusSpeed { get; set; }
        public double CPUTotalLoad { get; set; }
        public double CPUPackageTemp { get; set; }
        public List<double> CPUCoreClock { get; set; }
        public List<double> CPUCoreTemp { get; set; }
        public List<double> CPUCoreLoad { get; set; }
        public List<string> CPUPowers { get; set; }
        public string CPUPackagePow { get; set; }
        public string CPUCoresPow { get; set; }
        public string CPUGraphicsPow { get; set; }
        public List<string> Other { get; set; }
    }
}
