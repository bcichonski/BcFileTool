using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BcFileTool.Library.Streams
{
    public class HashStream : Stream
    {
        Stream _underlyingStream;
        HashAlgorithm _hashAlgorithm;

        public string Hash
        {
            get
            {
                if(Position == Length)
                {
                    _hashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
                    return Convert.ToBase64String(_hashAlgorithm.Hash);
                } else
                {
                    throw new Exception("Read stream to the end to retrieve hash");
                }
            }
        }

        public HashStream(Stream sourceStream, HashAlgorithm hashAlgorithm)
        {
            _underlyingStream = sourceStream;
            _hashAlgorithm = hashAlgorithm;
        }

        public override bool CanRead => _underlyingStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => _underlyingStream.Length;

        public override long Position { get => _underlyingStream.Position; set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var readBytes = _underlyingStream.Read(buffer, offset, count);

            _hashAlgorithm.TransformBlock(buffer, offset, count, buffer, offset);

            return readBytes;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing)
            {
                _underlyingStream.Dispose();
            }
        }
    }
}
