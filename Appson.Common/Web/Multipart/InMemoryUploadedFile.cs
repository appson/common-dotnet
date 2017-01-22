using System.IO;
using System.Net.Http.Headers;

namespace JahanJooy.Common.Util.Web.Multipart
{
    public class InMemoryUploadedFile
    {
        public InMemoryUploadedFile(HttpContentHeaders headers, MemoryStream stream)
        {
            Headers = headers;
            Stream = stream;
        }

        public HttpContentHeaders Headers { get; private set; }

        public MemoryStream Stream { get; private set; }
 
    }
}