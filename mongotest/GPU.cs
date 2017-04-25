using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongotest
{
    public class GPU
    {
        public List<string> GPUClocks { get; set; }
        public List<string> GPULoads { get; set; }
        public List<string> GPUData{ get; set; }

        public string GPUName { get; set; }
        public string GPUCore { get; set; }
        public string GPUMemory { get; set; }
        public string GPUShader { get; set; }
        public string GPUCoreTemp { get; set; }
        public string GPUCoreLoad { get; set; }
        public string GPUMemoryContLoad { get; set; }
        public string GPUVEngineLoad { get; set; }
        public string GPUMemoryLoad { get; set; }
        public string GPUMemoryFree { get; set; }
        public string GPUMemoryUsed { get; set; }
        public string GPUMemoryTotal { get; set; }
    }
}
