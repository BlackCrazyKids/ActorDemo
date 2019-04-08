using System;
using System.IO;

namespace Game
{
    public partial class NetworkChannel
    {
        private sealed class ReceiveState : IDisposable
        {
            private MemoryStream m_Stream;
            private BinaryReader m_Reader;
            private bool m_Disposed;

            public ReceiveState()
            {
                m_Stream = new MemoryStream(DefaultBufferLength);
                m_Reader = new BinaryReader(m_Stream);
                m_Disposed = false;
            }

            public MemoryStream Stream
            {
                get
                {
                    return m_Stream;
                }
            }
            public BinaryReader Reader
            {
                get
                {
                    return m_Reader;
                }
            }

            public void PrepareForPacketHeader(int packetHeaderLength)
            {
                Reset(packetHeaderLength);
            }

            public void PrepareForPacket(int packetBodyLength)
            {
                Reset(packetBodyLength);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (m_Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    if (m_Stream != null)
                    {
                        m_Reader.Close();
                        m_Stream.Dispose();
                        m_Stream = null;
                        m_Reader = null;
                    }
                }

                m_Disposed = true;
            }

            private void Reset(int targetLength)
            {
                if (targetLength < 0)
                {
                    throw new Exception("Target length is invalid.");
                }

                m_Stream.Position = 0L;
                m_Stream.SetLength(targetLength);
            }
        }
    }
}
