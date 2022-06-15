using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, IProgress<double> progress, int bufferSize = 81920, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(destination, nameof(destination));

            if (!source.CanRead)
            {
                throw new ArgumentException("Has to be readable", nameof(source));
            }
            if (!destination.CanWrite)
            {
                throw new ArgumentException("Has to be writable", nameof(destination));
            }
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            byte[] buffer = new byte[bufferSize];
            double totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                totalBytesRead += bytesRead;
                progress.Report(totalBytesRead);
            }
        }
    }
}