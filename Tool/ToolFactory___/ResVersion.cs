using System;
using System.IO;
using System.Text;

namespace ToolFactory
{
    //计算路径下文件md5,格式:path,md5,size
    class ResVersion
    {
        const string _resmd5 = "resmd5.txt";
        const string _version = "version.txt";

        public ResVersion(string dir, string saveDir)
        {
            StringBuilder sb = new StringBuilder();
            var fs = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < fs.Length; i++)
            {
                string path = fs[i].Replace(dir, "").TrimStart('/').TrimStart('\\');
                string md5 = ComputeMD5File(fs[i]);
                FileInfo info = new FileInfo(fs[i]);
                sb.AppendFormat("{0},{1},{2}\n", Util.StandardlizePath(path), md5, info.Length);
                Util.Log(">" + path);
            }
            try
            {
                string resmd5 = string.Format("{0}\\{1}", saveDir, _resmd5);
                File.WriteAllText(resmd5, sb.ToString());

                string version = string.Format("{0}\\{1}", saveDir, _version);
                string oldVersion = File.ReadAllText(version).Trim();
                string[] nodes = oldVersion.Split(".".ToCharArray());
                long resVersion = Convert.ToInt64(nodes[1]);
                resVersion++;
                DateTime now = DateTime.Now;
                string time = string.Format("{0}{1:D2}{2:D2}{3:D2}", now.Year % 100, now.Month, now.Day, now.Hour);
                string newVersion = string.Format("{0}.{1}.{2}", nodes[0], resVersion, time);
                File.WriteAllText(version, newVersion);
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("[ResVersion]资源版本信息文件生成异常!\n{0}\n{1}", e.Message, e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        string ComputeMD5File(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Compute MD5 File Fail, Error:" + ex.Message);
            }
        }
    }
}
