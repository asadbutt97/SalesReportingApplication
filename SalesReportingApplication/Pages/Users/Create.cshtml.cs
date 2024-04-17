using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesReportingApplication.Models;

namespace SalesReportingApplication.Pages.Users
{
   
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:20273");

        }

        [BindProperty]
        public UserProfile User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var json = JsonSerializer.Serialize(User);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/sales/users", content);

                response.EnsureSuccessStatusCode();

                return RedirectToPage("/Users/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user. Please try again.");
                return Page();
            }
        }
    }
}
