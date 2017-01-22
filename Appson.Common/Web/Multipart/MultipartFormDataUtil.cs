using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace JahanJooy.Common.Util.Web.Multipart
{
    public static class MultipartFormDataUtil
    {
        public static async Task<InMemoryMultipartFormDataStreamProvider> ReadRequestContent(HttpContent content)
        {
            // Append two bytes to the end of the stream, to prevent the weird IOException issue
            // http://stackoverflow.com/a/17290999/187996

            Stream requestStream = await content.ReadAsStreamAsync();
            Stream concatenatedStream = new MemoryStream();

            requestStream.CopyTo(concatenatedStream);
            concatenatedStream.Seek(0, SeekOrigin.End);

            StreamWriter writer = new StreamWriter(concatenatedStream);
            writer.WriteLine();
            writer.Flush();
            concatenatedStream.Position = 0;

            StreamContent streamContent = new StreamContent(concatenatedStream);

            foreach (var header in content.Headers)
                streamContent.Headers.Add(header.Key, header.Value);

            var result = new InMemoryMultipartFormDataStreamProvider();
            await streamContent.ReadAsMultipartAsync(result);

            return result;
        }
    }
}