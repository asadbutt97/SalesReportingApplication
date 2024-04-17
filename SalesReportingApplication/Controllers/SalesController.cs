using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesReportingApplication.Models;
using SalesReportingApplication.Repository;


namespace SalesReportingApplication.Controllers
{

   

    [Route("[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SalesRepository _repository;

        public SalesController(IConfiguration configuration)
        {
            _repository = new SalesRepository(configuration);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _repository.AuthenticateUser(loginModel.Username, loginModel.Password).userProfile;
            if (user == null)
                return Unauthorized();
            HttpContext.Session.SetInt32("UserID", user.UserID);

            return Ok(new
            {
                user.UserID,
                user.UserName,
                user.LoginID,
                user.UserRole,
                user.ReportingManager
            });
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("users")]
        public IActionResult AddUser([FromBody] UserProfile user)
        {
            var userId = _repository.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new { userId }, user);
        }

        [HttpGet("users/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _repository.GetUserById(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("users/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserProfile user)
        {
            if (userId != user.UserID)
                return BadRequest();

            var success = _repository.UpdateUser(user);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var success = _repository.DeleteUser(userId);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("transactions")]
        public IActionResult GetAllTransactions()
        {
            var transactions = _repository.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpPost("transactions")]
        public IActionResult AddTransaction([FromBody] SalesTransaction transaction)
        {
            var transactionId = _repository.AddTransaction(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId }, transaction);
        }

        [HttpGet("transactions/{transactionId}")]
        public IActionResult GetTransactionById(int transactionId)
        {
            var transaction = _repository.GetTransactionById(transactionId);
            if (transaction == null)
                return NotFound();
            else
                transaction.UserProfile = _repository.GetUserById(transaction.TransactionUserID);

            return Ok(transaction);
        }

        [HttpPut("transactions/{transactionId}")]
        public IActionResult UpdateTransaction(int transactionId, [FromBody] SalesTransaction transaction)
        {
            if (transactionId != transaction.TransactionID)
                return BadRequest();

            var success = _repository.UpdateTransaction(transaction);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("transactions/{transactionId}")]
        public IActionResult DeleteTransaction(int transactionId)
        {
            var success = _repository.DeleteTransaction(transactionId);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("reports/yearlysummary/{year}")]
        public IActionResult GetYearlySummary(int year)
        {
            try
            {
                var yearlySummary = _repository.GetYearlySalesSummaryReport(year);
                return Ok(yearlySummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("reports/monthlysummary/{year}/{month}")]
        public IActionResult GetMonthlySummary(int year, int month)
        {
            try
            {
                var monthlySummary = _repository.GetMonthlySalesSummaryReport(year, month);
                return Ok(monthlySummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
