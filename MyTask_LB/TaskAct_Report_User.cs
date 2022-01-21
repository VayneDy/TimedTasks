using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTimedTasks;

namespace MyTask_LB
{
    /// <summary>
    /// 定时任务，用户统计
    /// </summary>
    public class TaskAct_Report_User : TaskAct_Base
    {

        public TaskAct_Report_User() : base()
        {

        }
        protected override void Do()
        {
            base.Do();
            //do something
            //SqlHelper.ExecuteProcNonQuery("prLB_Report_User", null);
            Console.WriteLine(DateTime.Now.ToString());
            base.LogDebug(DateTime.Now.ToString());

        }


    }
}
