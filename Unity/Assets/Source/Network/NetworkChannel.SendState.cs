using System;
using System.IO;

namespace Game
{
    public partial class NetworkChannel
    {
        private sealed class SendState : IDisposable
        {
            private MemoryStream m_Stream;
            private BinaryWriter m_Writer;
            private bool m_Disposed;

            public SendState()
            {
                m_Stream = new MemoryStream(DefaultBufferLength);
                m_Writer = new BinaryWriter(m_Stream);
                m_Disposed = false;
            }

            public MemoryStream Stream
            {
                get
                {
                    return m_Stream;
                }
            }
            public BinaryWriter Writer
            {
                get
                {
                    return m_Writer;
                }
            }
            public void Reset()
            {
                m_Stream.Position = 0L;
                m_Stream.SetLength(0L);
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
                        m_Writer.Close();
                        m_Stream.Dispose();
                        m_Stream = null;
                        m_Writer = null;
                    }
                }

                m_Disposed = true;
            }
        }
    }
}
