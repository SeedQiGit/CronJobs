﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Model.Response
{
    public class BasePageResponse<T>
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 当前页数据集合
        /// </summary>
        public List<T> DataList { get; set; }
    }
}
