using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClientApp
{
    internal sealed class WalletWalletClient
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string token = "ww_live_7c03de085408f259610ad786cabc8ff4";
        public WalletWalletClient()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CallUsageAsync()
        {
            const string url = "https://api.walletwallet.dev/api/auth/usage";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreatePassAsync(string user = "PB Málaga", string dni = "12345678X", string telefono = "123456789")
        {
            const string url = "https://api.walletwallet.dev/api/passes";

            var safeUser = Uri.EscapeDataString(user ?? string.Empty);
            var safeTelefono = Uri.EscapeDataString(telefono ?? string.Empty);
            var safeDni = Uri.EscapeDataString(dni ?? string.Empty);

            var payload = new
            {
                barcodeValue = $"https://josemrom1.github.io/Pbmalaga/generador.html?nombre={safeUser}&telefono={safeTelefono}&dni={safeDni}",
                barcodeFormat = "QR",
                logoText = "Peña Bética de Málaga",
                description = "Carnet de Socio",
                primaryFields = new[]
                {
                    new { label = "Usuario", value = user }
                },
                colorPreset = "green",
                expirationDays = 365
            };

            var json = JsonSerializer.Serialize(payload);

            Console.WriteLine("=== WalletWallet create pass API ===");
            Console.WriteLine($"curl -X POST {url} \\");
            Console.WriteLine("  -H \"Content-Type: application/json\" \\");
            Console.WriteLine("  -H \"Authorization: Bearer " + token + "\" \\");
            Console.WriteLine("  -d '" + json + "'");

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine("Response:");
            Console.WriteLine(content);

            try
            {
                using var document = JsonDocument.Parse(content);
                var root = document.RootElement;

                if (root.TryGetProperty("shareUrl", out var shareUrlElement) && shareUrlElement.ValueKind == JsonValueKind.String)
                {
                    return shareUrlElement.GetString() ?? string.Empty;
                }

                if (root.TryGetProperty("shareURL", out var shareURLElement) && shareURLElement.ValueKind == JsonValueKind.String)
                {
                    return shareURLElement.GetString() ?? string.Empty;
                }
            }
            catch (JsonException)
            {
            }

            return string.Empty;
        }
    }
}
