using System;
using System.IO;

namespace Sinch.Core
{
    internal static class StreamExtensions
    {
        // NOTE: not used, may be used in send fax json request
        public static string ConvertToBase64(this Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return Convert.ToBase64String(memoryStream.ToArray());
            }

            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.ReadExactly(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }
    }
}
