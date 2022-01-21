

using MyTimedTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service_TimeTask : ServiceBase
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Service_TimeTask));
        
        
        public Service_TimeTask()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("服务启动");
            MyTaskFactory.Instence().Start();
        }

        protected override void OnStop()
        {
            log.Info("服务停止");
            
        }
    }
}
