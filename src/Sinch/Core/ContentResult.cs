using System;
using System.IO;
using System.Threading.Tasks;

namespace Sinch.Core
{
    public sealed class ContentResult : IDisposable, IAsyncDisposable
    {
        /// <summary>
        ///     The Stream containing data of the file
        /// </summary>
        public Stream Stream { get; init; } = null!;

        /// <summary>
        ///     Name of the file, if available.
        /// </summary>
        public string? FileName { get; init; }

        public void Dispose()
        {
            Stream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await Stream.DisposeAsync();
        }
    }
}
