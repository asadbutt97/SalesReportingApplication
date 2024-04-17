using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesReportingApplication.Models;
using System.Text;
using System.Text.Json;

namespace SalesReportingApplication.Pages.SalesTransactions
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:20273");
        }

        [BindProperty]
        public SalesTransaction Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/sales/transactions/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            Transaction = JsonSerializer.Deserialize<SalesTransaction>(json);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var json = JsonSerializer.Serialize(Transaction);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/sales/transactions/{Transaction.TransactionID}", content);
                response.EnsureSuccessStatusCode();

                return RedirectToPage("/SalesTransactions/Index");
            }
            catch (Exception ex)
            {
            
                ModelState.AddModelError(string.Empty, "An error occurred while updating the sales transaction. Please try again.");
                return Page();
            }
        }
    }
}
