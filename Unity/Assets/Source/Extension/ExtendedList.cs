using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ExtendedList
{
    /// <summary>
    /// 集合转换字符串
    /// </summary>
    /// <param name="list">集合</param>
    /// <param name="split">分隔符</param>
    /// <returns></returns>
    public static string List2String<T>(this List<T> list, string split = "\r\n")
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count - 1; i++)
            sb.AppendFormat("{0}{1}", list[i], split);
        sb.Append(list[list.Count - 1]);
        return sb.ToString();
    }
}
