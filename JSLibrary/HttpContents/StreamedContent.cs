﻿using JSLibrary.Extensions;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.HttpContents
{
    public sealed class StreamedContent : HttpContent
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Stream _stream;
        private readonly IProgress<double> _progress;

        public StreamedContent(Stream fileStream, IProgress<double> progress, CancellationToken cancellationToken)
        {
            _stream = fileStream;
            _progress = progress;
            _cancellationToken = cancellationToken;
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return Task.FromResult<Stream>(new ContentStream(_stream, _progress));
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await _stream.CopyToAsync(stream, _progress, cancellationToken: _cancellationToken);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }

        private class ContentStream : StreamWrapper
        {
            private readonly IProgress<double> _progress;
            private long _position = 0;

            public ContentStream(Stream stream, IProgress<double> progress)
                : base(stream)
            {
                _progress = progress;
            }

            public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                var readBytes = await base.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
                _position += readBytes;
                _progress?.Report(_position / Length * 100.0);
                return readBytes;
            }
        }
    }

    internal class StreamWrapper : Stream
    {
        public StreamWrapper(Stream stream)
        {
            InnerStream = stream;
        }

        public Stream InnerStream { get; init; }

        public override bool CanRead
        {
            get
            {
                return InnerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return InnerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return InnerStream.CanWrite;
            }
        }

        public bool TryGetPosition(out long? position)
        {
            if (!InnerStream.CanSeek)
            {
                position = null;
                return false;
            }
            try
            {
                position = InnerStream.Position;
                return true;
            }
            catch
            {
                position = null;
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return InnerStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return InnerStream.Position;
            }

            set
            {
                InnerStream.Position = value;
            }
        }

        public override void Flush()
        {
            InnerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return await InnerStream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return await InnerStream.ReadAsync(buffer, cancellationToken);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            await base.WriteAsync(buffer.AsMemory(offset, count), cancellationToken);
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await base.WriteAsync(buffer, cancellationToken);
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await InnerStream.FlushAsync(cancellationToken);
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            await InnerStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            InnerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            InnerStream.Dispose();
        }
    }
}