using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimedTasks
{
    public class CommonFn
    {


        /// <summary>
        /// 获取当前程序根目录
        /// </summary>
        /// <returns></returns>
        public static string GetRootDir()
        {
            var rootpath = "";
            try
            {
                rootpath = AppDomain.CurrentDomain.BaseDirectory;
                if (string.IsNullOrEmpty(rootpath))
                {
                    rootpath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                }
            }
            catch (Exception)
            {

            }
            return rootpath;
        }

        

    }
}
