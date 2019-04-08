using System;
using System.Text;
using System.IO;
using System.Reflection;
namespace Cfg
{
	/// <summary>
	/// 数据解析类
	/// <summary>
	public class DataStream
	{
		private readonly string[] _line;
		private int _index;
		public DataStream(string path, Encoding encoding)
		{
			_line = File.ReadAllLines(path);
			_index = 0;
		}
		
		public string GetNext()
		{
			return _index < _line.Length ? _line[_index++] : null;
		}
		
		private void Error(string err)
		{
			throw new Exception(err);
		}
		
		private string GetNextAndCheckNotEmpty()
		{
			string v = GetNext();
			if (v == null) {
				Error("read not enough");
			}
			return v;
		}
		
		public string GetString()
		{
			return GetNextAndCheckNotEmpty();
		}
		public float GetFloat()
		{
			return float.Parse(GetNextAndCheckNotEmpty());
		}
		public int GetInt()
		{
			return int.Parse(GetNextAndCheckNotEmpty());
		}
		public long GetLong()
		{
			return long.Parse(GetNextAndCheckNotEmpty());
		}
		public bool GetBool()
		{
			string v = GetNextAndCheckNotEmpty();
			if (v == "true") {
				return true;
			}
			if (v == "false") {
				return false;
			}
			Error(v + " isn't bool");
			return false;
		}
		public Cfg.CfgObject GetObject(string name)
		{
			return (Cfg.CfgObject)Type.GetType(name).GetConstructor(new[] { typeof(Cfg.DataStream) }).Invoke(new object[] { this });		}
	}
}
