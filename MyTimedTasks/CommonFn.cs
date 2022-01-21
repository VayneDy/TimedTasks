using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimedTasks
{
    public class CommonFn
    {
        

        //获取程序根目录
        public static string GetRootDir()
        {
            var rootpath = "";
            try
            {
                rootpath = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception)
            {

            }
            if (string.IsNullOrEmpty(rootpath))
            {
                rootpath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            return rootpath;
        }

        /// <summary>
        /// 实体转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToStr<T>(T t)
        {
            try
            {
                var dicts = t.GetType().GetProperties().ToDictionary(q => q.Name, q => q.GetValue(t));
                var modelAttrArr = new List<string>();
                foreach (var item in dicts)
                {
                    modelAttrArr.Add($"{item.Key}={item.Value}");
                }
                var modelAttrStr = string.Join("&", modelAttrArr);
                return modelAttrStr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
