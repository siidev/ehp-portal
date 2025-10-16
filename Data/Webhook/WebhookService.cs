
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSOPortalX.Data.Webhook
{
    public class WebhookService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WebhookService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendWebhookAsync(string webhookUrl, string secret, object payload)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(3);

            var jsonPayload = JsonSerializer.Serialize(payload);
            var signature = CalculateHmacSignature(secret, jsonPayload);

            var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl);
            request.Headers.Add("X-Signature", signature);
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // TODO: Implement a retry mechanism for failed webhooks (e.g., using Polly)
            try
            {
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine($"Webhook failed for {webhookUrl}: {(int)response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Webhook failed for {webhookUrl}: {e.Message}");
            }
        }

        private string CalculateHmacSignature(string secret, string payload)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return System.Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
