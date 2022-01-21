using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimedTasks
{
    /// <summary>
    /// 定时任务基类，添加业务逻辑任务类后继承此类
    /// </summary>
    public class TaskAct_Base
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TaskAct_Base));
        //子类模块名称，用于写日志，方便查看是那个业务类输出的日志
        protected string ModelName;
        /* 执行类型
         * 1：每天在ExcuteStartTime定时执行一次
         * 2：大于ExcuteStartTime开始无限循环执行
         * 3：每天在ExcuteStartTime到ExcuteEndTime时间段内循环执行
         * 4: 每月执行一次
         * **/
        protected int ExcuteType;
        //每月几号执行
        protected int ExcuteDayPerMonth;
        //开始执行时间 HH:mm
        protected string ExcuteStartTime;
        //结束执行时间 HH:mm
        protected string ExcuteEndTime;
        //循环执行时的循环间隔
        protected int ExcuteInterval;
        //循环执行时的循环间隔的单位, m :分钟, s : 秒
        protected string ExcuteIntervalType;
        //下一次的执行时间
        internal DateTime NextExcuteTime;
        //定时器
        protected System.Timers.Timer timer;

        public TaskAct_Base() { }
        internal void Start(TaskModel model)
        {
            this.ModelName = model.ModelName;
            this.ExcuteType = int.Parse(model.ExcuteType);
            this.ExcuteStartTime = model.StartTime;
            this.ExcuteEndTime = model.EndTime;
            this.ExcuteInterval = string.IsNullOrEmpty(model.Interval) ? 5 : int.Parse(model.Interval);
            this.ExcuteIntervalType = string.IsNullOrEmpty(model.IntervalType) ? "m" : model.IntervalType;
            this.ExcuteDayPerMonth = string.IsNullOrEmpty(model.ExcuteDayPerMonth) ? 1 : int.Parse(model.ExcuteDayPerMonth);
            InitialNextStartTime();
            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000 * 1;
            timer.Enabled = true;
        }
        //初始化设定下一次执行时间
        private void InitialNextStartTime()
        {
            NextExcuteTime = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {ExcuteStartTime}");
            if (ExcuteType == 1)
            {
                if (DateTime.Now > NextExcuteTime)
                {
                    NextExcuteTime = NextExcuteTime.AddDays(1);
                }
            }
            else if (ExcuteType == 2)
            {
                if (DateTime.Now > NextExcuteTime)
                {
                    NextExcuteTime = DateTime.Now;
                }
            }
            else if (ExcuteType == 3)
            {
                DateTime endTime = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {ExcuteEndTime}");
                if (DateTime.Now > endTime)
                {
                    NextExcuteTime = NextExcuteTime.AddDays(1);
                }
                else
                {
                    if (DateTime.Now > NextExcuteTime)
                    {
                        NextExcuteTime = DateTime.Now;
                    }
                }
            }
            else if (ExcuteType == 4)
            {
                NextExcuteTime = DateTime.Parse($"{DateTime.Now:yyyy-MM}-{ExcuteDayPerMonth} {ExcuteStartTime}");
                if (DateTime.Now > NextExcuteTime)
                {
                    NextExcuteTime = NextExcuteTime.AddMonths(1);
                }
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Enabled = false;
                CheckTime();
            }
            catch (Exception ex)
            {
                log.Error($"定时任务,{ModelName} CheckTime 失败：{ex.Message}");
            }
            finally
            {
                timer.Enabled = true;
            }
        }
        //检查是否到达下一次执行时间
        private void CheckTime()
        {
            if (DateTime.Now < NextExcuteTime) return;
            if (ExcuteType == 1)
            {
                Do1();
                NextExcuteTime = NextExcuteTime.AddDays(1);
            }
            else if (ExcuteType == 2)
            {
                Do1();
                if (ExcuteIntervalType == "s") NextExcuteTime = NextExcuteTime.AddSeconds(ExcuteInterval);
                else if (ExcuteIntervalType == "m") NextExcuteTime = NextExcuteTime.AddMinutes(ExcuteInterval);
                else if(ExcuteIntervalType == "h") NextExcuteTime = NextExcuteTime.AddHours(ExcuteInterval);
                else NextExcuteTime = NextExcuteTime.AddMinutes(ExcuteInterval);
            }
            else if (ExcuteType == 3)
            {
                DateTime startTime = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {ExcuteStartTime}");
                DateTime endTime = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {ExcuteEndTime}");
                if (DateTime.Now >= startTime && DateTime.Now <= endTime)
                {
                    Do1();
                    if (ExcuteIntervalType == "s") NextExcuteTime = NextExcuteTime.AddSeconds(ExcuteInterval);
                    else if (ExcuteIntervalType == "m") NextExcuteTime = NextExcuteTime.AddMinutes(ExcuteInterval);
                    else if (ExcuteIntervalType == "h") NextExcuteTime = NextExcuteTime.AddHours(ExcuteInterval);
                    else NextExcuteTime = NextExcuteTime.AddMinutes(ExcuteInterval);
                    if (NextExcuteTime > endTime)
                    {
                        NextExcuteTime = startTime.AddDays(1);
                    }
                }
            }
            else if (ExcuteType == 4)
            {
                Do1();
                NextExcuteTime = NextExcuteTime.AddMonths(1);
            }
            log.Info($"定时任务,{ModelName},执行完成,下一次执行时间：{NextExcuteTime}");
        }
        //包一层，记录日志
        private void Do1()
        {
            try
            {
                log.Info($"定时任务,{ModelName},开始执行");
                Do();
                log.Info($"定时任务,{ModelName},执行成功");
            }
            catch (Exception ex)
            {
                log.Error($"定时任务, {ModelName},出现异常：{ex.Message}");
                log.Info($"定时任务,{ModelName},执行失败");
            }
        }
        
        /// <summary>
        /// 执行具体任务，子类重写此方法，写具体业务逻辑
        /// </summary>
        protected virtual void Do()
        {

        }

        //记录日志，子类直接用父类的记日志方法，不用再实例化log4net对象
        protected void LogInfo(string msg)
        {
            log.Info($"{ModelName},{msg}");
        }
        protected void LogDebug(string msg)
        {
            log.Debug($"{ModelName},{msg}");
        }
        protected void LogError(string msg)
        {
            log.Error($"{ModelName},{msg}");
        }
        protected void LogWarn(string msg)
        {
            log.Warn($"{ModelName},{msg}");
        }

    }
}
