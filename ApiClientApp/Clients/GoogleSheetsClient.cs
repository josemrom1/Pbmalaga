using ApiClientApp.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiClientApp
{
    public sealed class GoogleSheetsClient
    {
        private readonly SheetsService _service;
        private readonly string? _credentialsJsonPath;
        private readonly string? _apiKey;

        public GoogleSheetsClient(string? credentialsJsonPath = null, string? apiKey = null)
        {
            _credentialsJsonPath = credentialsJsonPath;
            _apiKey = apiKey;

            _service = CreateService(requireWriteAccess: false);
        }

        private SheetsService CreateService(bool requireWriteAccess)
        {
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                return new SheetsService(new BaseClientService.Initializer
                {
                    ApiKey = _apiKey,
                    ApplicationName = "PbmalagaApiClient"
                });
            }

            if (string.IsNullOrWhiteSpace(_credentialsJsonPath))
            {
                throw new InvalidOperationException("Provide either a service account JSON path or an API key.");
            }

            if (!File.Exists(_credentialsJsonPath))
            {
                throw new FileNotFoundException("Google Sheets credentials file was not found.", _credentialsJsonPath);
            }

            using var stream = new FileStream(_credentialsJsonPath, FileMode.Open, FileAccess.Read);
            var scope = requireWriteAccess
                ? SheetsService.Scope.Spreadsheets
                : SheetsService.Scope.SpreadsheetsReadonly;

            var credential = GoogleCredential.FromStream(stream)
                .CreateScoped(scope);

            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "PbmalagaApiClient"
            });
        }

        public async Task<IList<Penista>> ReadRangeAsync(string spreadsheetId, string range)
        {
            if (string.IsNullOrWhiteSpace(spreadsheetId))
            {
                throw new ArgumentException("Spreadsheet ID cannot be empty.", nameof(spreadsheetId));
            }

            if (string.IsNullOrWhiteSpace(range))
            {
                throw new ArgumentException("Range cannot be empty.", nameof(range));
            }

            var request = _service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();

            var rows = response.Values ?? new List<IList<object>>();
            if (rows.Count == 0)
            {
                return new List<Penista>();
            }

            var headers = rows[0].Select(value => value?.ToString() ?? string.Empty).ToList();

            return rows.Skip(1)
                .Select(row => new Penista
                {
                    Nombre = GetValue(row, headers, "Nombre"),
                    Telefono = GetValue(row, headers, "Telefono"),
                    DNI = GetValue(row, headers, "DNI"),
                    GoogleURL = GetValue(row, headers, "GoogleURL"),
                    AppleURL = GetValue(row, headers, "AppleURL"),
                    ShareURL = GetValue(row, headers, "ShareURL")
                })
                .ToList();
        }

        private async Task<IList<IList<object>>> ReadRawRangeAsync(string spreadsheetId, string range)
        {
            if (string.IsNullOrWhiteSpace(spreadsheetId))
            {
                throw new ArgumentException("Spreadsheet ID cannot be empty.", nameof(spreadsheetId));
            }

            if (string.IsNullOrWhiteSpace(range))
            {
                throw new ArgumentException("Range cannot be empty.", nameof(range));
            }

            var request = _service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();

            return response.Values ?? new List<IList<object>>();
        }

        public async Task<int> UpdateRowByPhoneAsync(string spreadsheetId, string range, string telefono, string url, string targetColumnName = "GoogleURL")
        {
            if (string.IsNullOrWhiteSpace(spreadsheetId))
            {
                throw new ArgumentException("Spreadsheet ID cannot be empty.", nameof(spreadsheetId));
            }

            if (string.IsNullOrWhiteSpace(range))
            {
                throw new ArgumentException("Range cannot be empty.", nameof(range));
            }

            if (string.IsNullOrWhiteSpace(telefono))
            {
                throw new ArgumentException("Phone number cannot be empty.", nameof(telefono));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be empty.", nameof(url));
            }

            var rows = await ReadRawRangeAsync(spreadsheetId, range);
            if (rows.Count == 0)
            {
                throw new InvalidOperationException("No rows were found in the provided range.");
            }

            var headerRow = rows[0].Select(value => value?.ToString() ?? string.Empty).ToList();
            int phoneColumnIndex = FindColumnIndex(headerRow, "Telefono");
            if (phoneColumnIndex < 0)
            {
                phoneColumnIndex = 1;
            }

            int targetColumnIndex = FindColumnIndex(headerRow, targetColumnName);
            if (targetColumnIndex < 0)
            {
                targetColumnIndex = 5;
            }

            int rowNumber = -1;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Count > phoneColumnIndex &&
                    string.Equals(rows[i][phoneColumnIndex]?.ToString(), telefono, StringComparison.OrdinalIgnoreCase))
                {
                    rowNumber = i + 1;
                    break;
                }
            }

            if (rowNumber < 0)
            {
                throw new InvalidOperationException($"No row found for phone number '{telefono}'.");
            }

            var writableService = CreateService(requireWriteAccess: true);
            string sheetName = range.Contains("!") ? range.Split('!')[0] : "Sheet1";
            string escapedSheetName = sheetName.Contains(" ") || sheetName.Contains("'")
                ? $"'{sheetName.Replace("'", "''")}'"
                : sheetName;

            string columnName = ToColumnName(targetColumnIndex);
            string cellAddress = $"{escapedSheetName}!{columnName}{rowNumber}";

            var updateRequest = writableService.Spreadsheets.Values.Update(
                new ValueRange
                {
                    Values = new List<IList<object>> { new List<object> { url } }
                },
                spreadsheetId,
                cellAddress);

            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            await updateRequest.ExecuteAsync();

            return rowNumber;
        }

        private static string? GetValue(IList<object> row, IReadOnlyList<string> headers, string columnName)
        {
            int index = FindColumnIndex(headers, columnName);
            if (index < 0 || index >= row.Count)
            {
                return null;
            }

            return row[index]?.ToString();
        }

        private static int FindColumnIndex(IReadOnlyList<string> headers, string columnName)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                if (string.Equals(headers[i], columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        private static string ToColumnName(int index)
        {
            index += 1;

            string columnName = string.Empty;
            while (index > 0)
            {
                index--;
                columnName = (char)('A' + (index % 26)) + columnName;
                index /= 26;
            }

            return columnName;
        }
    }
}
