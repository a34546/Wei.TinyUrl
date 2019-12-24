using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wei.Repository;

namespace Wei.TinyUrl.Data.Entities
{
    public class UrlMapping : Entity
    {
        /// <summary>
        /// 生成的短链接Code
        /// </summary>
        [StringLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 需要转换的链接
        /// </summary>
        [StringLength(2000)]
        public string Url { get; set; }

        /// <summary>
        /// 过期时间（为null时是永久）
        /// </summary>
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(20)]
        public string Source { get; set; }
    }
}
