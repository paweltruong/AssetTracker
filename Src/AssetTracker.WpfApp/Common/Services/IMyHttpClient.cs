using System.Diagnostics.CodeAnalysis;
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

        Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
    }
}
