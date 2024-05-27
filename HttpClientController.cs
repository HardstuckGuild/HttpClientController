using System.Globalization;
using System.Net;

namespace Hardstuck.Http
{
    /// <summary>
    /// Modified HttpClient class with extra methods and setups.
    /// </summary>
    public class HttpClientController : HttpClient
    {
        #region definitions
        /// <summary>
        /// The version of the HttpClientController.
        /// </summary>
        public static float Version => 1.02f;

        /// <summary>
        /// Create an instance of HttpClientController, child of HttpClient.
        /// </summary>
        public HttpClientController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("HsClientController", Version.ToString("0.00", CultureInfo.InvariantCulture)));
        }
        #endregion

        /// <summary>
        /// Downloads a file asynchronously.
        /// </summary>
        /// <param name="url">URL to download from.</param>
        /// <param name="destination">Destination to download to.</param>
        /// <returns>Boolean whether the file was downloaded successfully.</returns>
        public async Task<bool> DownloadFileAsync(string url, string destination)
        {
            try
            {
                var uri = new Uri(url);
                using var responseMessage = await GetAsync(uri);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return false;
                }
                using var response = await responseMessage.Content.ReadAsStreamAsync();
                using var stream = File.Create(@destination);
                await response.CopyToAsync(stream);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Downloads a file to a string asynchronously.
        /// </summary>
        /// <param name="url">URL to download from.</param>
        /// <returns>The response as a string.</returns>
        public async Task<string?> DownloadFileToStringAsync(string url)
        {
            try
            {
                var uri = new Uri(url);
                using var responseMessage = await GetAsync(uri);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return null;
                }
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
