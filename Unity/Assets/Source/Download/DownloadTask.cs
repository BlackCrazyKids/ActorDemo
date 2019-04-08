using System.IO;
using System.Net;
using UnityEngine;

namespace Game
{
    internal class AssetInfo
    {
        public string RelPath;
        public string MD5;
        public long Size;

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", RelPath, MD5, Size);
        }
    } 

    public partial class UpdateManager
    {

        internal class DownloadTask
        {
            public static long ChunkSize = 4096L;
            private string _uri;
            private AssetInfo _remoteInfo;
            private UpdateManager _updater;

            public DownloadTask(string uri, AssetInfo remoteInfo, UpdateManager updater)
            {
                _uri = uri;
                _remoteInfo = remoteInfo;
                _updater = updater;
            }

            public void BeginDownload()
            {
                string dir = Util.DataPath + Path.GetDirectoryName(_remoteInfo.RelPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                try
                {
                    string file = Util.DataPath + _remoteInfo.RelPath;
                    if (File.Exists(file))
                        File.Delete(file);

                    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(_uri));
                    httpWebRequest.Timeout = 2 * 60 * 1000;
                    httpWebRequest.KeepAlive = false;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    byte[] buffer = new byte[ChunkSize];
                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        int readTotalSize = 0;
                        int size = stream.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            //只将读出的字节写入文件
                            fs.Write(buffer, 0, size);
                            readTotalSize += size;
                            size = stream.Read(buffer, 0, buffer.Length);
                        }
                    }
                    httpWebRequest.Abort();
                    httpWebRequest = null;
                    httpWebResponse.Close();
                    httpWebResponse = null;

                    _updater._taskCompleted.Enqueue(() =>
                    {
                        string md5 = string.Empty;
                        if (File.Exists(file))
                            md5 = Util.ComputeMD5File(file);

                        if (md5.Equals(_remoteInfo.MD5))
                        {
                            _updater._downloadSize += _remoteInfo.Size;
                            _updater._hasDownload.Add(_remoteInfo.RelPath);
                            _updater.AppendHasDownloadFile(_remoteInfo.RelPath);
                            _updater.AppendMd5File(_remoteInfo);
                        }
                        else
                        {
                            _updater._redownloadList.Add(_remoteInfo.RelPath);
                        }
                    });
                }
                catch (System.Exception e)
                {
                    Debug.LogFormat("Download File Error = {0}, Exception = {1}", _uri, e.Message);
                    _updater._taskCompleted.Enqueue(() =>
                    {
                        _updater._redownloadList.Add(_remoteInfo.RelPath);
                    });
                }
            }
        }

        private void AppendMd5File(AssetInfo remoteInfo)
        {
           
            string file = Util.DataPath + _resMD5FileRelPath;
            try
            {
                using (var writer = new StreamWriter(new FileStream(file, FileMode.Append)))
                {
                    writer.WriteLine(remoteInfo.ToString());
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        private void AppendHasDownloadFile(string relPath)
        {
            string file = Util.DataPath + _hasDownloadFileRelPath;
            try
            {
                using (var writer = new StreamWriter(new FileStream(file, FileMode.Append)))
                {
                    writer.WriteLine(relPath);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}

