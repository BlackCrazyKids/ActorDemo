using System.IO;

namespace Game
{
    /// <summary>
    /// 网络消息包头接口
    /// |Type|Length|Msg
    /// </summary>
    public class NetworkChannelHelper
    {
        /// <summary>
        /// 固定包头长度
        /// </summary>
        public virtual int HeaderLength
        {
            get { return 8; }
        }

        /// <summary>
        /// 获取网络消息包长度。
        /// </summary>
        public int MsgLength
        {
            get;
            private set;
        }
        /// <summary>
        /// 网络消息类型
        /// </summary>
        public int MsgType
        {
            get;
            private set;
        }

        public bool HasReadHeader
        {
            get;
            private set;
        }


        public NetworkChannelHelper() { }

        public virtual void DecodeHeader(BinaryReader reader)
        {
            MsgType = reader.ReadInt32();
            MsgLength = reader.ReadInt32();
            HasReadHeader = true;
        }
        /// <summary>
        /// 解码协议
        /// </summary>
        public virtual Protocol Decode(BinaryReader reader)
        {
            HasReadHeader = false;
            return new Protocol(MsgType, reader.ReadBytes(MsgLength));
        }
        /// <summary>
        /// 编码协议
        /// </summary>
        public virtual void EnCode(BinaryWriter writer, Protocol protocol)
        {
            writer.Write(protocol.Type);
            writer.Write(protocol.Msg.Length);
            writer.Write(protocol.Msg, 0, protocol.Msg.Length);
        }
    }
}
