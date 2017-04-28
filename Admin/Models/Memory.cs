using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin
{
    public class Memory
    {
        public string Name { get; set; }
        public double MemoryLoad { get; set; }
        public List<double> UsedMemory { get; set; }
        
    }
}
