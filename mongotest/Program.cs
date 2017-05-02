using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Hardware;
using OxyPlot;
using OxyPlot.Series;
using MongoDB.Bson.Serialization;
using Topshelf;


namespace mongotest
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Model>(service =>
                {
                    service.ConstructUsing(srv => new Model());
                    service.WhenStarted(tc => tc.Start());
                    service.WhenStopped(tc => tc.Stop());
                    
                    
                });
                x.SetServiceName("ATHService");
                x.SetDisplayName("AthService");
                x.StartAutomatically();
                //x.UseNLog();
                x.RunAsLocalSystem();
            });


            
        }


    }
}
