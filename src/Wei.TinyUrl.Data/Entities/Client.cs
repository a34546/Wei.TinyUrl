using System;
using System.Collections.Generic;
using System.Text;

namespace Wei.TinyUrl.Data.Entities
{
    public class Client
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        public int? Days { get; set; }
    }
}
