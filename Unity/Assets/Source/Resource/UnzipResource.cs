using System.IO;
using UnityEngine;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;

namespace Game
{
    public partial class ResourceManager
    {
        byte[] _dataZip = null;
        string _dataPath = null;
        public IEnumerator CheckUnzipData()
        {
#if UNITY_EDITOR && !GAME_SIMULATION
            yield break;
#endif
            if (Client.UpdateMgr.HasdownloadFile)
                yield break;

            string zip = Util.StreamingPath + "../data.zip";
            Debug.Log("解压data:" + zip);
            WWW www = new WWW(zip);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("解压data.zip失败!\n" + www.error);
                yield break;
            }

            _dataZip = www.bytes;
            _dataPath = Util.DataPath + "../";
            www.Dispose();
            www = null;
            Thread thread = new Thread(UnzipRaw);
            thread.Start();

            while (_dataZip != null)
                yield return null;

            thread.Abort();
        }

        private void UnzipRaw()
        {
            MemoryStream stream = new MemoryStream(_dataZip);
            using (ZipFile zfile = new ZipFile(stream))
            {
                long count = zfile.Count;
                int i = 0;

                string resmd5 = "Data/" + ConstSetting.ResMD5File;
                string version = "Data/" + ConstSetting.ResVersionFile;
                string hasDownload = "Data/" + ConstSetting.HasDownloadFile;

                var iter = zfile.GetEnumerator();
                while (iter.MoveNext())
                {
                    var entry = iter.Current as ZipEntry;
                    string filePath = entry.Name;
                    if (string.IsNullOrEmpty(filePath))
                        continue;

                    if (filePath == resmd5)
                        filePath = _dataPath + ConstSetting.ResMD5File;
                    else if (filePath == version)
                        filePath = _dataPath + ConstSetting.ResVersionFile;
                    else if (filePath == hasDownload)
                        filePath = _dataPath + ConstSetting.HasDownloadFile;
                    else
                        filePath = _dataPath + filePath;

                    Debug.LogFormat("{0}", filePath);
                    string subDirectoryName = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(subDirectoryName) && !Directory.Exists(subDirectoryName))
                    {
                        Directory.CreateDirectory(subDirectoryName);
                    }

                    string fileName = Path.GetFileName(filePath);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        i++;
                        continue;
                    }

                    using (FileStream streamWriter = File.Create(filePath))
                    {
                        int size = 2048;
                        byte[] data = new byte[size];
                        Stream zipStream = zfile.GetInputStream(entry);
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    Launcher.Ins.SetLaunchState(LaunchState.CheckUnzipData, ++i * 1f / count);
                }
                zfile.Close();
            }

            _dataZip = null;
            _dataPath = null;
        }

        private void ClearDataPath()
        {
            string[] dirs = Directory.GetDirectories(Util.DataPath);
            for (int i = 0; i < dirs.Length; i++)
                Directory.Delete(dirs[i], true);
            string[] fs = Directory.GetFiles(Util.DataPath);
            for (int i = 0; i < fs.Length; i++)
                File.Delete(fs[i]);
        }
    }
}
