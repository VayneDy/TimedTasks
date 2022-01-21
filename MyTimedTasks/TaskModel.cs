using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimedTasks
{
   internal class TaskModel
    {
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string ModelName { get; set; }
        public string ExcuteType { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Interval { get; set; }
        public string IntervalType { get; set; }
        public string Enabled { get; set; }
        public string ExcuteDayPerMonth { get; set; }

    }
}
