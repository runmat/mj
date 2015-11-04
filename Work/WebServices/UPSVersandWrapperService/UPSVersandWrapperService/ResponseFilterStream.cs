using System;
using System.IO;

namespace UPSVersandWrapperService
{
    public class ResponseFilterStream : Stream
    {
        private readonly Stream InnerStream;
        private readonly MemoryStream CopyStream;

        public ResponseFilterStream(Stream inner)
        {
            InnerStream = inner;
            CopyStream = new MemoryStream();
        }

        public override void Flush()
        {
            InnerStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            CopyStream.Seek(offset, origin);
            return InnerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            CopyStream.SetLength(value);
            InnerStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            CopyStream.Write(buffer, offset, count);
            InnerStream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return InnerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return InnerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return InnerStream.CanWrite; }
        }

        public override long Length
        {
            get { return InnerStream.Length; }
        }

        public override long Position
        {
            get { return InnerStream.Position; }
            set
            {
                CopyStream.Position = value;
                InnerStream.Position = value;
            }
        }

        public string ReadStream()
        {
            lock (InnerStream)
            {
                if (CopyStream.Length < 0 || !CopyStream.CanRead || !CopyStream.CanSeek)
                {
                    return String.Empty;
                }

                var pos = CopyStream.Position;
                CopyStream.Position = 0;

                try
                {
                    return new StreamReader(CopyStream).ReadToEnd();
                }
                finally
                {
                    try
                    {
                        CopyStream.Position = pos;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}