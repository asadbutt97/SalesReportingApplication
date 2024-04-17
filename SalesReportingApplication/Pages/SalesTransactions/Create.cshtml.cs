using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesReportingApplication.Models;
using System.Text;
using System.Text.Json;
using System.Transactions;

namespace SalesReportingApplication.Pages.SalesTransactions
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:20273");
        }

        public List<UserProfile> Users { get; set; }
        [BindProperty]
        public SalesTransaction Transaction { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                Users = await _httpClient.GetFromJsonAsync<List<UserProfile>>("sales/users");
                return Page();
            }
            catch (Exception ex)
            {
            
                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string transactionUserID = Request.Form["transactionUserID"];

                if (!transactionUserID.Equals(""))
                {
                    Transaction.TransactionUserID = Convert.ToInt32(transactionUserID);
                }
                var json = JsonSerializer.Serialize(Transaction);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/sales/transactions", content);


                if (response.IsSuccessStatusCode)
                {
                  
                    return RedirectToPage("/SalesTransactions/Index");
                }
                else
                {
                 
                    return RedirectToPage("/Error");
                }
            }
            catch (Exception ex)
            {
       
                return RedirectToPage("/Error");
            }
        }
    }
}

