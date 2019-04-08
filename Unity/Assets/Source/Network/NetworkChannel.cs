using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 网络类型。
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// IP 版本 4。
        /// </summary>
        IPv4,

        /// <summary>
        /// IP 版本 6。
        /// </summary>
        IPv6,
    }


    public partial class NetworkChannel : INetworkChannel, IDisposable
    {

        private const int DefaultBufferLength = 1024 * 8;
        private const float DefaultHeartBeatInterval = 30f;

        private const int ChannelConnectedState = -100;
        private const int ChannelClosedState = -200;

        private readonly string m_Name;
        private readonly Queue<Protocol> m_SendPacketPool;
        private readonly Queue<Protocol> m_ReceivePacketPool;
        private NetworkType m_NetworkType;
        private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
        private float m_HeartBeatInterval;
        private Socket m_Socket;
        private readonly NetworkChannelHelper m_Helper;
        private readonly SendState m_SendState;
        private readonly ReceiveState m_ReceiveState;
        private readonly HeartBeatState m_HeartBeatState;
        private int m_SentPacketCount;
        private int m_ReceivedPacketCount;
        private bool m_Active;
        private bool m_Disposed;

        public Action<NetworkChannel> NetworkChannelConnected;
        public Action<NetworkChannel> NetworkChannelClosed;
        public Action<NetworkChannel, int> NetworkChannelMissHeartBeat;
        public Action<NetworkChannel, NetworkErrorCode, string> NetworkChannelError;
        public Action<int, object> NetworkReceive;

        private bool m_IsJson = false;


        /// <summary>
        /// 初始化网络频道的新实例。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        public NetworkChannel(string name)
        {
            m_Name = name ?? string.Empty;
            m_SendPacketPool = new Queue<Protocol>();
            m_ReceivePacketPool = new Queue<Protocol>();
            m_NetworkType = NetworkType.Unknown;
            m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
            m_HeartBeatInterval = DefaultHeartBeatInterval;
            m_Socket = null;
            m_Helper = new NetworkChannelHelper();
            m_SendState = new SendState();
            m_ReceiveState = new ReceiveState();
            m_HeartBeatState = new HeartBeatState();
            m_SentPacketCount = 0;
            m_ReceivedPacketCount = 0;
            m_Active = false;
            m_Disposed = false;
        }

        /// <summary>
        /// 获取网络频道名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取是否已连接。
        /// </summary>
        public bool Connected
        {
            get
            {
                if (m_Socket != null)
                {
                    return m_Socket.Connected;
                }

                return false;
            }
        }

        /// <summary>
        /// 获取网络类型。
        /// </summary>
        public NetworkType NetworkType
        {
            get
            {
                return m_NetworkType;
            }
        }

        /// <summary>
        /// 获取本地终结点的 IP 地址。
        /// </summary>
        public IPAddress LocalIPAddress
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.LocalEndPoint;
                if (ipEndPoint == null)
                {
                    throw new Exception("Local end point is invalid.");
                }

                return ipEndPoint.Address;
            }
        }

        /// <summary>
        /// 获取本地终结点的端口号。
        /// </summary>
        public int LocalPort
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.LocalEndPoint;
                if (ipEndPoint == null)
                {
                    throw new Exception("Local end point is invalid.");
                }

                return ipEndPoint.Port;
            }
        }

        /// <summary>
        /// 获取远程终结点的 IP 地址。
        /// </summary>
        public IPAddress RemoteIPAddress
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.RemoteEndPoint;
                if (ipEndPoint == null)
                {
                    throw new Exception("Remote end point is invalid.");
                }

                return ipEndPoint.Address;
            }
        }

        /// <summary>
        /// 获取远程终结点的端口号。
        /// </summary>
        public int RemotePort
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.RemoteEndPoint;
                if (ipEndPoint == null)
                {
                    throw new Exception("Remote end point is invalid.");
                }

                return ipEndPoint.Port;
            }
        }

        /// <summary>
        /// 获取要发送的消息包数量。
        /// </summary>
        public int SendPacketCount
        {
            get
            {
                return m_SendPacketPool.Count;
            }
        }

        /// <summary>
        /// 获取累计发送的消息包数量。
        /// </summary>
        public int SentPacketCount
        {
            get
            {
                return m_SentPacketCount;
            }
        }

        /// <summary>
        /// 获取已接收未处理的消息包数量。
        /// </summary>
        public int ReceivePacketCount
        {
            get
            {
                return m_ReceivePacketPool.Count;
            }
        }

        /// <summary>
        /// 获取累计已接收的消息包数量。
        /// </summary>
        public int ReceivedPacketCount
        {
            get
            {
                return m_ReceivedPacketCount;
            }
        }

        /// <summary>
        /// 获取或设置当收到消息包时是否重置心跳流逝时间。
        /// </summary>
        public bool ResetHeartBeatElapseSecondsWhenReceivePacket
        {
            get
            {
                return m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            }
            set
            {
                m_ResetHeartBeatElapseSecondsWhenReceivePacket = value;
            }
        }

        /// <summary>
        /// 获取丢失心跳的次数。
        /// </summary>
        public int MissHeartBeatCount
        {
            get
            {
                return m_HeartBeatState.MissHeartBeatCount;
            }
        }

        /// <summary>
        /// 获取或设置心跳间隔时长，以秒为单位。
        /// </summary>
        public float HeartBeatInterval
        {
            get
            {
                return m_HeartBeatInterval;
            }
            set
            {
                m_HeartBeatInterval = value;
            }
        }

        /// <summary>
        /// 获取心跳等待时长，以秒为单位。
        /// </summary>
        public float HeartBeatElapseSeconds
        {
            get
            {
                return m_HeartBeatState.HeartBeatElapseSeconds;
            }
        }

        /// <summary>
        /// 获取或设置接收缓冲区字节数。
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                return m_Socket.ReceiveBufferSize;
            }
            set
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                m_Socket.ReceiveBufferSize = value;
            }
        }

        /// <summary>
        /// 获取或设置发送缓冲区字节数。
        /// </summary>
        public int SendBufferSize
        {
            get
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                return m_Socket.SendBufferSize;
            }
            set
            {
                if (m_Socket == null)
                {
                    throw new Exception("You must connect first.");
                }

                m_Socket.SendBufferSize = value;
            }
        }

        /// <summary>
        /// 网络频道轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Socket == null || !m_Active)
            {
                return;
            }

            ProcessSend();
            while (m_ReceivePacketPool.Count > 0)
            {
                //转发至Lua层
                Protocol packet = m_ReceivePacketPool.Dequeue();
                if (NetworkReceive != null)
                {

                    if (packet.Type > 0)
                    {
                        if (m_IsJson)
                            NetworkReceive(packet.Type, Encoding.UTF8.GetString(packet.Msg, 0, packet.Msg.Length));
                        else
                            NetworkReceive(packet.Type, packet.Msg);
                    }
                    else
                    {
                        switch (packet.Type)
                        {
                            case ChannelConnectedState:
                                NetworkChannelConnected(this);
                                break;
                            case ChannelClosedState:
                                NetworkChannelClosed(this);
                                break;
                            default:
                                Debug.LogError(Name + "无法将消息转发至lua层");
                                break;
                        }
                    }

                    if (m_HeartBeatInterval > 0f)
                    {
                        bool sendHeartBeat = false;
                        int missHeartBeatCount = 0;
                        lock (m_HeartBeatState)
                        {
                            m_HeartBeatState.HeartBeatElapseSeconds += realElapseSeconds;
                            if (m_HeartBeatState.HeartBeatElapseSeconds >= m_HeartBeatInterval)
                            {
                                sendHeartBeat = true;
                                missHeartBeatCount = m_HeartBeatState.MissHeartBeatCount;
                                m_HeartBeatState.HeartBeatElapseSeconds = 0f;
                                m_HeartBeatState.MissHeartBeatCount++;
                            }
                        }

                        if (sendHeartBeat && SendHeartBeat())
                        {
                            if (missHeartBeatCount > 0 && NetworkChannelMissHeartBeat != null)
                            {
                                NetworkChannelMissHeartBeat(this, missHeartBeatCount);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        public void Connect(string ip, int port)
        {
            int size = DefaultBufferLength * 2;
            Connect(ip, port, size, size, null);
        }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void Connect(string ip, int port, int sendBuffer, int receiveBuffer, object userData)
        {
            if (m_Socket != null)
            {
                Close();
                m_Socket = null;
            }

            IPAddress ipAddress = IPAddress.Parse(ip);
            switch (ipAddress.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    m_NetworkType = NetworkType.IPv4;
                    break;
                case AddressFamily.InterNetworkV6:
                    m_NetworkType = NetworkType.IPv6;
                    break;
                default:
                    string errorMessage = string.Format("Not supported address family '{0}'.", ipAddress.AddressFamily.ToString());
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.AddressFamilyError, errorMessage);
                        return;
                    }

                    throw new Exception(errorMessage);
            }

            m_Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.SendBufferSize = sendBuffer;
            m_Socket.ReceiveBufferSize = receiveBuffer;
            m_Socket.NoDelay = true;
            if (m_Socket == null)
            {
                string errorMessage = "Initialize network channel failure.";
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SocketError, errorMessage);
                    return;
                }

                throw new Exception(errorMessage);
            }

            m_SendState.Reset();
            m_ReceiveState.PrepareForPacketHeader(m_Helper.HeaderLength);

            try
            {
                m_Socket.BeginConnect(ipAddress, port, ConnectCallback, new ConnectState(m_Socket, userData));
            }
            catch (Exception exception)
            {
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.ConnectError, exception.Message);
                    return;
                }

                throw;
            }
        }

        /// <summary>
        /// 关闭连接并释放所有相关资源。
        /// </summary>
        public void Close()
        {
            lock (this)
            {
                if (m_Socket == null)
                {
                    return;
                }

                lock (m_SendPacketPool)
                {
                    m_SendPacketPool.Clear();
                }

                m_ReceivePacketPool.Clear();

                m_Active = false;
                m_SentPacketCount = 0;
                m_ReceivedPacketCount = 0;
                try
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                finally
                {
                    m_Socket.Close();
                    m_Socket = null;

                    if (NetworkChannelClosed != null)
                    {
                        m_ReceivePacketPool.Enqueue(new Protocol(ChannelClosedState, null));
                    }
                }
            }
        }

        /// <summary>
        /// 向远程主机发送消息包。
        /// </summary>
        /// <param name="packet">要发送的消息包.直接在lua层封装.</param>
        public void Send(int type, byte[] msg)
        {
            m_IsJson = false;
            if (m_Socket == null)
            {
                string errorMessage = "You must connect first.";
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SendError, errorMessage);
                    return;
                }

                throw new Exception(errorMessage);
            }

            if (!m_Active)
            {
                string errorMessage = "Socket is not active.";
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SendError, errorMessage);
                    return;
                }

                throw new Exception(errorMessage);
            }

            lock (m_SendPacketPool)
            {
                m_SendPacketPool.Enqueue(new Protocol(type, msg));
            }
        }
        /// <summary>
        /// Json形式发送数据
        /// </summary>
        public void Send(int type, string msg)
        {
            Send(type, System.Text.Encoding.UTF8.GetBytes(msg));
            m_IsJson = true;
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                Close();
                m_SendState.Dispose();
                m_ReceiveState.Dispose();

                NetworkChannelConnected = null;
                NetworkChannelClosed = null;
                NetworkChannelMissHeartBeat = null;
                NetworkChannelError = null;
                NetworkReceive = null;
            }

            m_Disposed = true;
        }

        private bool SendHeartBeat()
        {
            return true;
        }
        private void Send()
        {
            try
            {
                m_Socket.BeginSend(m_SendState.Stream.GetBuffer(), (int)m_SendState.Stream.Position, (int)(m_SendState.Stream.Length - m_SendState.Stream.Position), SocketFlags.None, SendCallback, m_Socket);
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SendError, exception.Message);
                    return;
                }

                throw;
            }
        }

        private void Receive()
        {
            try
            {
                m_Socket.BeginReceive(m_ReceiveState.Stream.GetBuffer(),
                    (int)m_ReceiveState.Stream.Position, (int)(m_ReceiveState.Stream.Length - m_ReceiveState.Stream.Position),
                    SocketFlags.None, ReceiveCallback, m_Socket);
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.ReceiveError, exception.Message);
                    return;
                }

                throw;
            }
        }

        private void ProcessSend()
        {
            if (m_SendState.Stream.Length > 0 || m_SendPacketPool.Count <= 0)
            {
                return;
            }

            while (m_SendPacketPool.Count > 0)
            {
                try
                {
                    m_Helper.EnCode(m_SendState.Writer, m_SendPacketPool.Dequeue());
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SerializeError, exception.ToString());
                        return;
                    }
                    throw;
                }
            }

            m_SendState.Stream.Position = 0L;

            Send();
        }

        private bool ProcessPacketHeader()
        {
            try
            {
                m_Helper.DecodeHeader(m_ReceiveState.Reader);

                if (m_Helper.MsgType < 0)
                {
                    string errorMessage = "Packet header is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError, errorMessage);
                        return false;
                    }

                    throw new Exception(errorMessage);
                }

                m_ReceiveState.PrepareForPacket(m_Helper.MsgLength);
                if (m_Helper.MsgLength <= 0)
                {
                    //空消息,直接解析Body
                    ProcessPacketBody();
                }
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError, exception.ToString());
                    return false;
                }

                throw;
            }

            return true;
        }

        private bool ProcessPacketBody()
        {
            lock (m_HeartBeatState)
            {
                m_HeartBeatState.Reset(m_ResetHeartBeatElapseSecondsWhenReceivePacket);
            }

            try
            {
                Protocol packet = m_Helper.Decode(m_ReceiveState.Reader);
                m_ReceivePacketPool.Enqueue(packet);

                m_ReceiveState.PrepareForPacketHeader(m_Helper.HeaderLength);
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.DeserializePacketError, exception.ToString());
                    return false;
                }

                throw;
            }

            return true;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            ConnectState socketUserData = (ConnectState)ar.AsyncState;
            try
            {
                socketUserData.Socket.EndConnect(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.ConnectError, exception.Message);
                    return;
                }

                throw;
            }

            m_Active = true;
            m_SentPacketCount = 0;
            m_ReceivedPacketCount = 0;

            lock (m_HeartBeatState)
            {
                m_HeartBeatState.Reset(true);
            }

            if (NetworkChannelConnected != null)
            {
                m_ReceivePacketPool.Enqueue(new Protocol(ChannelConnectedState, null));
            }

            Receive();
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int bytesSent = 0;
            try
            {
                bytesSent = socket.EndSend(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.SendError, exception.Message);
                    return;
                }

                throw;
            }

            m_SendState.Stream.Position += bytesSent;
            if (m_SendState.Stream.Position < m_SendState.Stream.Length)
            {
                Send();
                return;
            }

            m_SentPacketCount++;
            m_SendState.Reset();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int bytesReceived = 0;
            try
            {
                bytesReceived = socket.EndReceive(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (Exception exception)
            {
                m_Active = false;
                if (NetworkChannelError != null)
                {
                    NetworkChannelError(this, NetworkErrorCode.ReceiveError, exception.Message);
                    return;
                }

                throw;
            }

            if (bytesReceived <= 0)
            {
                Close();
                return;
            }

            m_ReceiveState.Stream.Position += bytesReceived;
            if (m_ReceiveState.Stream.Position < m_ReceiveState.Stream.Length)
            {
                Receive();
                return;
            }

            m_ReceivedPacketCount++;
            m_ReceiveState.Stream.Position = 0L;

            bool processSuccess = false;
            if (!m_Helper.HasReadHeader)
                processSuccess = ProcessPacketHeader();
            else
                processSuccess = ProcessPacketBody();

            if (processSuccess)
            {
                Receive();
                return;
            }
        }
    }
}
