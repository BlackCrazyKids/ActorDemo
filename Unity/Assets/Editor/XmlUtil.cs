using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class XmlUtil
{
    /// <summary>
    /// Xml序列化
    /// </summary>
    public static void Serialize(string filePath, object sourceObj)
    {
        if (!string.IsNullOrEmpty(filePath) && filePath.Length != 0 && sourceObj != null)
        {
            File.Delete(filePath);
            Type type = sourceObj.GetType();
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                xmlSerializer.Serialize(streamWriter, sourceObj);
            }
        }
    }
    /// <summary>
    /// Xml反序列化
    /// </summary>
    public static object Deserialize(string filePath, Type type)
    {
        object result = null;
        if (File.Exists(filePath))
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    result = new XmlSerializer(type).Deserialize(streamReader);
                }
            }
            catch (Exception e)
            {
                string txt = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(txt))
                {
                    File.Delete(filePath);
                    UnityEngine.Debug.LogError(filePath + "空文件!");
                }
                UnityEngine.Debug.LogErrorFormat("{2}\nActor Xml Exception: {0}\n{1}", e.Message, e.StackTrace, filePath);
            }
        }
        return result;
    }
}
