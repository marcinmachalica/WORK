using MongoDB.Bson;
using mongotest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ATHService
{
    public partial class Service1 : ServiceBase
    {
        static bool exist = false;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ObjectId tmpid = new ObjectId();
            

            var hw = new HardwareModel();

            hw.createFirst(hw);
            exist = true;
            tmpid = hw._id;

        }


        protected override void OnStop()
        {
        }

        protected override void OnContinue()
        {
            ObjectId tmpid = new ObjectId();
            var hw = new HardwareModel();
            if (exist)
            {
                hw.replaceDocument(hw, tmpid);
            }
        }
    }
}
