using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BackendService;

namespace WindowsService
{
    public partial class BackendWindowsService : ServiceBase
    {
        ServiceHost sHost;
        public BackendWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            sHost = new ServiceHost(typeof(Server.ServerServices));
            sHost.Open();
        }

        protected override void OnStop()
        {
            sHost.Close();
        }
    }
}
