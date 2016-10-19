using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxList.Models
{
    public class PrintLabel
    {
        /// <summary>
        /// 基础的条形码图片路径
        /// </summary>
        public string BarCodeImagePath { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// EU尺码
        /// </summary>
        public string EUSize { get; set; }
        /// <summary>
        /// AUS尺码
        /// </summary>
        public string AUSSize { get; set; }
        /// <summary>
        /// USA尺码
        /// </summary>
        public string USASize { get; set; }
        /// <summary>
        /// CM尺码
        /// </summary>
        public string CMSize { get; set; }
        /// <summary>
        /// INCHES尺码
        /// </summary>
        public string INCHESSize { get; set; }

    }
}
