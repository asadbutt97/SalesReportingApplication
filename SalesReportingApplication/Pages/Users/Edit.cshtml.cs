using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesReportingApplication.Models;

namespace SalesReportingApplication.Pages.Users
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
        public UserProfile User { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Fetch user data from API
            var response = await _httpClient.GetAsync($"/sales/users/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            User = JsonSerializer.Deserialize<UserProfile>(json);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var json = JsonSerializer.Serialize(User);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/sales/users/{User.UserID}", content);
              
                response.EnsureSuccessStatusCode();

                return RedirectToPage("/Users/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the user. Please try again.");
                return Page();
            }
        }
    }
}
