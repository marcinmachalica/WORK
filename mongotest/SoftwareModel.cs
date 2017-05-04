using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace mongotest
{
    public class SoftwareModel
    {
        public List<Soft> Programs { get; set; }
        public List<Proces> Processes { get; set; }
        public List<Hotfix> Hotfixes { get; set; }



        public SoftwareModel()
        {
            Programs = new List<Soft>();
            Processes = new List<Proces>();
            Hotfixes = new List<Hotfix>();

            if (File.Exists("C:\\\\ATHService\\Programs.csv") && File.Exists("C:\\\\ATHService\\Processes.csv") && File.Exists("C:\\\\ATHService\\Hotfixes.csv"))
            {
                try
                {

                    using (var fs = File.OpenRead(@"C:\ATHService\Programs.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');


                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }


                            Soft Prog = new Soft()
                            {
                                IdentifyingNumber = values[0],
                                Name = values[1],
                                Version = values[3],
                                Vendor = values[2],
                                Description = values[5],
                                Caption = values[4],
                                Remove = false
                            };
                            Programs.Add(Prog);
                        }
                    }

                    using (var fs = File.OpenRead(@"C:\ATHService\Processes.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }
                            Proces Proc = new Proces()
                            {
                                ProcesId = values[1],
                                ProcesName = values[0]
                            };
                            Processes.Add(Proc);
                        }
                    }

                    using (var fs = File.OpenRead(@"C:\ATHService\Hotfixes.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }
                            Hotfix Hotfix = new Hotfix()
                            {
                                Description = values[0],
                                HotFixID = values[1],
                                InstalledOn = values[2]
                            };
                            Hotfixes.Add(Hotfix);
                        }
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    using (PowerShell PowerShellInstance = PowerShell.Create())
                    {
                        PowerShellInstance.AddScript("Get-WmiObject -Class Win32_Product |Sort-Object Name | select IdentifyingNumber,Name,Vendor,Version,Caption,Description | export-csv C:\\\\ATHService\\Programs.csv");
                        PowerShellInstance.AddScript("Get-WmiObject -Class Win32_QuickFixEngineering | select Description,HotFixID,InstalledOn |export-csv C:\\\\ATHService\\Hotfixes.csv");
                        PowerShellInstance.AddScript("get-process | Sort-Object ProcessName | select ProcessName,Id | export-csv C:\\\\ATHService\\Processes.csv");
                        PowerShellInstance.Invoke();
                        System.Threading.Thread.Sleep(30000);
                        
                    }
                    
                    using (var fs = File.OpenRead(@"C:\ATHService\Programs.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }
                            Soft Prog = new Soft()
                            {
                                IdentifyingNumber = values[0],
                                Name = values[1],
                                Version = values[3],
                                Vendor = values[2],
                                Description = values[5],
                                Caption = values[4],
                                Remove = false
                            };
                            Programs.Add(Prog);
                        }
                    }

                    using (var fs = File.OpenRead(@"C:\ATHService\Processes.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }
                            Proces Proc = new Proces()
                            {
                                ProcesId = values[1],
                                ProcesName = values[0]
                            };
                            Processes.Add(Proc);
                        }
                    }

                    using (var fs = File.OpenRead(@"C:\ATHService\Hotfixes.csv"))
                    using (var reader = new StreamReader(fs))
                    {
                        var headerline = reader.ReadLine();
                        headerline = reader.ReadLine();
                        while (!reader.EndOfStream)
                        {

                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            for (int i = 0; i < values.Count(); i++)
                            {
                                values[i] = values[i].Replace("\\", "").Replace("\"", "");
                            }
                            Hotfix Hotfix = new Hotfix()
                            {
                                Description = values[0],
                                HotFixID = values[1],
                                InstalledOn = values[2]
                            };
                            Hotfixes.Add(Hotfix);
                        }
                    }
                }
                catch { }
            }
        }
    }
}
