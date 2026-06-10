using ApiClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClientApp
{
    internal class Program
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly WalletWalletClient WalletWallet = new WalletWalletClient();

        private static async Task Main(string[] args)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("=== C# API Client Demo ===");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("=== Google Sheets (private sheet) ===");
            Console.WriteLine("Replace the placeholders below with your service-account JSON path, spreadsheet ID, and sheet range.");

            try
            {
                // For a private Google Sheet, use the service-account JSON file.
                // Example:
                // var credentialsPath = "/absolute/path/to/your-service-account.json";
                // var spreadsheetId = "YOUR_SPREADSHEET_ID";
                // var range = "Hoja1!A1:C10";

                var credentialsPath = "/Users/nereamartinez/ProyectosJose/Pbmalaga/ApiClientApp/wedding-1365-45138626e2bf.json";
                var spreadsheetId = "1Ess-ySS0dEUamM_78igL1wuOztn-630lIcRsyY3OzkQ";
                var range = "Hoja 1!A1:z999";

                if (credentialsPath.Contains("CHANGE_ME") || spreadsheetId.Contains("CHANGE_ME"))
                {
                    Console.WriteLine("Pending setup:");
                    Console.WriteLine("  1) Replace 'CHANGE_ME_TO_YOUR_SERVICE_ACCOUNT_JSON_PATH' with the full path to your Google service-account JSON file.");
                    Console.WriteLine("  2) Replace 'CHANGE_ME_TO_YOUR_SPREADSHEET_ID' with the Google Sheets spreadsheet ID.");
                    Console.WriteLine("  3) Optionally change the range if you want another section of the sheet.");
                }
                else
                {
                    var sheets = new GoogleSheetsClient(credentialsJsonPath: credentialsPath);
                    var records = await sheets.ReadRangeAsync(spreadsheetId, range);

                    foreach (var record in records)
                    {
                        await WalletWallet.CallUsageAsync();
                        var passURL = await WalletWallet.CreatePassAsync(record.Nombre, record.DNI);

                        var sheetsClient = new GoogleSheetsClient(credentialsJsonPath: credentialsPath);
                        var updatedRow = await sheetsClient.UpdateRowByPhoneAsync(
                            spreadsheetId,
                            range,
                            telefono: record.Telefono,
                            url: passURL,
                            targetColumnName: "ShareURL");

                        Console.WriteLine($"Updated row {updatedRow} with the ShareURL value.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google Sheets example failed: {ex.Message}");
            }
        }
    }
}
