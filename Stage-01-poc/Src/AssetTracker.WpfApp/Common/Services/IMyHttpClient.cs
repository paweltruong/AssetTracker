using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;

namespace AssetTracker.WpfApp.Common.Services
{
    /// <summary>
    /// Wrapper interface for HttpClient to facilitate testing.
    /// </summary>
    public interface IMyHttpClient
    {
        Uri? BaseAddress { get; set; }
        TimeSpan Timeout { get; set; }

        #region Simple Get Overloads

        Task<string> GetStringAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        Task<string> GetStringAsync(Uri? requestUri);
        Task<string> GetStringAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken);
        Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken);
        //Task<string> GetStringAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken);
        Task<byte[]> GetByteArrayAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        Task<byte[]> GetByteArrayAsync(Uri? requestUri);
        Task<byte[]> GetByteArrayAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken);
        Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken);
        //Task<byte[]> GetByteArrayAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken);
        Task<Stream> GetStreamAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        Task<Stream> GetStreamAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken);
        Task<Stream> GetStreamAsync(Uri? requestUri);
        Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken);
        //Task<Stream> GetStreamAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken);

        #endregion Simple Get Overloads

        #region REST Send Overloads

        Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        Task<HttpResponseMessage> GetAsync(Uri? requestUri);
        Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content);
        Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        Task<HttpResponseMessage> DeleteAsync(Uri? requestUri);
        Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken);

        #endregion REST Send Overloads
    }
}
