using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace Boutique.Client.Services
{
    // Export service for generating Excel, PDF reports and data backups
    public class ExportService
    {
        // JavaScript runtime for browser-based export functionality
        private readonly IJSRuntime _jsRuntime;

        // Constructor with dependency injection for JavaScript runtime
        public ExportService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Export generic data list to Excel file using JavaScript
        public async Task ExportToExcelAsync<T>(List<T> data, string fileName)
        {
            try
            {
                // Serialize data to JSON for JavaScript processing
                var json = JsonSerializer.Serialize(data);
                // Call JavaScript function to generate Excel file
                await _jsRuntime.InvokeVoidAsync("exportToExcel", json, fileName);
            }
            catch (Exception ex)
            {
                // Log export errors for debugging
                Console.WriteLine($"Export to Excel error: {ex.Message}");
            }
        }

        // Export HTML content to PDF file using JavaScript
        public async Task ExportToPdfAsync(string content, string fileName)
        {
            try
            {
                // Call JavaScript function to generate PDF from HTML content
                await _jsRuntime.InvokeVoidAsync("exportToPdf", content, fileName);
            }
            catch (Exception ex)
            {
                // Log export errors for debugging
                Console.WriteLine($"Export to PDF error: {ex.Message}");
            }
        }

        // Generate formatted receipt PDF from payment/order data
        public async Task GenerateReceiptPdfAsync(object receiptData, string fileName)
        {
            try
            {
                // Serialize receipt data for JavaScript processing
                var json = JsonSerializer.Serialize(receiptData);
                // Call JavaScript function to generate formatted receipt PDF
                await _jsRuntime.InvokeVoidAsync("generateReceiptPdf", json, fileName);
            }
            catch (Exception ex)
            {
                // Log receipt generation errors
                Console.WriteLine($"Generate receipt PDF error: {ex.Message}");
            }
        }

        // Backup all application data using JavaScript
        public async Task BackupDataAsync()
        {
            try
            {
                // Call JavaScript function to create data backup
                await _jsRuntime.InvokeVoidAsync("backupData");
            }
            catch (Exception ex)
            {
                // Log backup errors for debugging
                Console.WriteLine($"Backup data error: {ex.Message}");
            }
        }
    }
}