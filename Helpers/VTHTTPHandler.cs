using System.Net.Http;
using System.Threading.Tasks;

using VTDownloader.Enums;
using VTDownloader.Objects;

namespace VTDownloader.Helpers
{
    public class VTHTTPHandler
    {
        public static async Task<DownloadResponseItem> DownloadAsync(string vtKey, string hash)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("x-apikey", vtKey);

                    var file = await httpClient.GetByteArrayAsync(
                        $"https://www.virustotal.com/api/v3/files/{hash}/download");

                    return new DownloadResponseItem(file);
                }
            } catch (HttpRequestException requestException)
            {
                switch (requestException.StatusCode)
                {
                    case System.Net.HttpStatusCode.Forbidden:
                        return new DownloadResponseItem(DownloadResponseStatus.INVALID_VT_KEY);
                    case System.Net.HttpStatusCode.NotFound:
                        return new DownloadResponseItem(DownloadResponseStatus.SAMPLE_NOT_FOUND);
                    default:
                        return new DownloadResponseItem(DownloadResponseStatus.UNEXPECTED_HTTP_ERROR, requestException);
                }
            }
        }
    }
}