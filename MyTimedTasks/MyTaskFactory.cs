using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimedTasks
{
    /// <summary>
    /// 定时任务执行入口，用法：MyTaskFactory.Instence().Start();
    /// </summary>
    public class MyTaskFactory
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MyTaskFactory));
        private static readonly MyTaskFactory instence = null;
        private List<TaskModel> tasks = new List<TaskModel>();
        static MyTaskFactory()
        {
            instence = new MyTaskFactory();
        }
        private MyTaskFactory()
        {
            try
            {
                log.Info("开始读取定时任务配置文件");
                string path = CommonFn.GetRootDir() + "/XmlConfig/MyTasks.json";
                string json = File.ReadAllText(path, Encoding.Default);
                tasks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TaskModel>>(json);
            }
            catch (Exception ex)
            {
                log.Error("定时任务,读取配置文件失败：" + ex.Message);
            }
        }
        public static MyTaskFactory Instence()
        {
            return instence;
        }
        public void Start()
        {
            log.Info("开始创建定时任务");
            tasks.Where(p => p.Enabled == "Y").ToList().ForEach(p => Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var task = System.Reflection.Assembly.LoadFrom(CommonFn.GetRootDir() + p.AssemblyName).CreateInstance(p.ClassName) as TaskAct_Base;
                        task.Start(p);
                        log.Info($"创建任务成功：{JsonConvert.SerializeObject(p)},下一次执行时间：{task.NextExcuteTime}");
                    }
                    catch (Exception ex)
                    {
                        log.Error("定时任务,创建任务失败：" + ex.Message);
                    }
                }));
        }

    }
}
