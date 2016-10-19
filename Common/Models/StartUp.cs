using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common.Models
{
    public struct StartUp
    {
        public StartUp(string InputParams)
        {
            string[] param = InputParams.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in param)
            {
                var temp = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                dic.Add(temp[0], temp[1]);
            }         
            StartUpConfig = dic["StartUpConfig"];
            IsBlog = bool.Parse(dic["IsBlog"]);
            StartIndex = int.Parse(dic["StartIndex"]);
            TopCount = int.Parse(dic["TopCount"]);
        }
        public string StartUpConfig;

        public bool IsBlog;

        public int StartIndex;

        public int TopCount;
        
    }    
}
