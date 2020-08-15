using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using VTDownloader.Enums;

namespace VTDownloader.Helpers
{
    public class VTHTTPHandler
    {
        public static async Task<DownloadResponse> DownloadAsync(string downloadLocation, string vtKey, string hash)
        {
            using (var httpClient = new HttpClient())
            {
                var file = await httpClient.GetByteArrayAsync(
                    $"https://www.virustotal.com/vtapi/v2/file/download?apikey={vtKey}&hash={hash}");

                if (file == null)
                {
                    return DownloadResponse.UNKNOWN_FILE;
                }

                await File.WriteAllBytesAsync(downloadLocation, file);

                return DownloadResponse.SUCCESS;
            }
        }
    }
}