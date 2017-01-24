using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Appson.Common.Web.Multipart
{
    /// <summary>
    /// A MultipartStreamProvider that keeps everything (including file contents) in the memory (using MemoryStream)
    /// Inspired by http://stackoverflow.com/questions/15842496/is-it-possible-to-override-multipartformdatastreamprovider-so-that-is-doesnt-sa
    /// </summary>
    public class InMemoryMultipartFormDataStreamProvider : MultipartStreamProvider
    {
        private readonly Collection<bool> _isFormData = new Collection<bool>();
        private readonly Collection<InMemoryUploadedFile> _fileData = new Collection<InMemoryUploadedFile>();

        public NameValueCollection FormData { get; } = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

        public Collection<InMemoryUploadedFile> FileData => _fileData;

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            // For form data, Content-Disposition header is a requirement
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                // If we have a file name then write contents out to AWS stream. Otherwise just write to MemoryStream
                if (!string.IsNullOrEmpty(contentDisposition.FileName))
                {
                    // We won't post process files as form data
                    _isFormData.Add(false);

                    var stream = new MemoryStream();
                    var fileData = new InMemoryUploadedFile(headers, stream);
                    _fileData.Add(fileData);

                    return stream;
                }

                // We will post process this as form data
                _isFormData.Add(true);

                // If no filename parameter was found in the Content-Disposition header then return a memory stream.
                return new MemoryStream();
            }

            throw new InvalidOperationException("Did not find required 'Content-Disposition' header field in MIME multipart body part..");
        }

        /// <summary>
        /// Read the non-file contents as form data.
        /// </summary>
        /// <returns></returns>
        public override async Task ExecutePostProcessingAsync()
        {
            // Find instances of HttpContent for which we created a memory stream and read them asynchronously
            // to get the string content and then add that as form data
            for (int index = 0; index < Contents.Count; index++)
            {
                if (_isFormData[index])
                {
                    HttpContent formContent = Contents[index];
                    // Extract name from Content-Disposition header. We know from earlier that the header is present.
                    ContentDispositionHeaderValue contentDisposition = formContent.Headers.ContentDisposition;
                    string formFieldName = UnquoteToken(contentDisposition.Name) ?? string.Empty;

                    // Read the contents as string data and add to form data
                    string formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
            }
        }

        /// <summary>
        /// Remove bounding quotes on a token if present
        /// </summary>
        /// <param name="token">Token to unquote.</param>
        /// <returns>Unquoted token.</returns>
        public static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }

}