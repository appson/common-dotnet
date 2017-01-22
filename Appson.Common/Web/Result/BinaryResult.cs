using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Appson.Common.Web.Result
{
    public class BinaryResult : IHttpActionResult
    {
        private readonly Stream _stream;
        private readonly string _contentType;

        public BinaryResult(Stream stream, string contentType = null)
        {
            if (stream == null) 
                throw new ArgumentNullException("stream");

            _stream = stream;
            _contentType = contentType;
        }

        public BinaryResult(byte[] bytes, string contentType = null)
            : this(new MemoryStream(bytes), contentType)
        {
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(_stream)
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType ?? "application/octet-stream");
                return response;
            }, cancellationToken);
        }
    }
}