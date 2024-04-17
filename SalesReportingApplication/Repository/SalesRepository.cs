namespace SalesReportingApplication.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Dapper;
    using Microsoft.Extensions.Configuration;

    using SalesReportingApplication.Models;

    public class SalesRepository
    {
        private readonly IDbConnection _dbConnection;

        public SalesRepository(IConfiguration configuration) => _dbConnection = new SqlConnection(configuration.GetConnectionString("Con"));

       
        public IEnumerable<UserProfile> GetAllUsers()
        {
            return _dbConnection.Query<UserProfile>("SELECT * FROM UserProfile");
        }

        public (bool success, UserProfile userProfile) AuthenticateUser(string username, string password)
        {
            var user = _dbConnection.QueryFirstOrDefault<UserProfile>("SELECT * FROM UserProfile WHERE LoginID = @Username AND Password = @Password", new { Username = username, Password = password });

            if (user != null)
            { 
                return (true, user);
            }
            else
            {
            
                return (false, null);
            }
        }

        public UserProfile GetUserById(int userId)
        {
            return _dbConnection.QueryFirstOrDefault<UserProfile>("SELECT * FROM UserProfile WHERE UserID = @UserId", new { UserId = userId });
        }

        public int AddUser(UserProfile user)
        {
            string sql = @"INSERT INTO UserProfile (UserName, LoginID, Password, UserRole, ReportingManager)
                        VALUES (@UserName, @LoginID, @Password, @UserRole, @ReportingManager);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            return _dbConnection.ExecuteScalar<int>(sql, user);
        }

        public bool UpdateUser(UserProfile user)
        {
            string sql = @"UPDATE UserProfile SET UserName = @UserName, LoginID = @LoginID, 
                        Password = @Password, UserRole = @UserRole, ReportingManager= @ReportingManager
                        WHERE UserID = @UserID";

            return _dbConnection.Execute(sql, user) > 0;
        }

        public bool DeleteUser(int userId)
        {
            string sql = @"DELETE FROM UserProfile WHERE UserID = @UserID";
            return _dbConnection.Execute(sql, new { UserID = userId }) > 0;
        }

       
        public IEnumerable<SalesTransaction> GetAllTransactions()
        {
            string query = @"SELECT st.*, up.* 
                     FROM SalesTransaction st
                     INNER JOIN UserProfile up ON st.TransactionUserID = up.UserID";

            return _dbConnection.Query<SalesTransaction, UserProfile, SalesTransaction>(query,
                (salesTransaction, userProfile) =>
                {
                    salesTransaction.UserProfile = userProfile;
                    return salesTransaction;
                },
                splitOn: "UserID");
        }

        public SalesTransaction GetTransactionById(int transactionId)
        {

            return _dbConnection.QueryFirstOrDefault<SalesTransaction>("SELECT * FROM SalesTransaction WHERE TransactionID = @TransactionId", new { TransactionId = transactionId });
        }

        public int AddTransaction(SalesTransaction transaction)
        {
            string sql = @"INSERT INTO SalesTransaction (SalesItem, SalesDate, TransactionUserID, Amount, UpdatedDate)
                        VALUES (@SalesItem, @SalesDate, @TransactionUserID, @Amount, @UpdatedDate);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            return _dbConnection.ExecuteScalar<int>(sql, transaction);
        }

        public bool UpdateTransaction(SalesTransaction transaction)
        {
            string sql = @"UPDATE SalesTransaction SET SalesItem = @SalesItem, SalesDate = @SalesDate,
                        TransactionUserID = @TransactionUserID, Amount = @Amount, UpdatedDate = @UpdatedDate
                        WHERE TransactionID = @TransactionID";

            return _dbConnection.Execute(sql, transaction) > 0;
        }

        public bool DeleteTransaction(int transactionId)
        {
            string sql = @"DELETE FROM SalesTransaction WHERE TransactionID = @TransactionID";
            return _dbConnection.Execute(sql, new { TransactionID = transactionId }) > 0;
        }

        public IEnumerable<SalesSummaryYearly> GetYearlySalesSummaryReport(int year)
        {
            return _dbConnection.Query<SalesSummaryYearly>("sp_GetYearlySalesSummary", new { Year = year }, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<SalesSummaryMonthly> GetMonthlySalesSummaryReport(int year, int month)
        {
            return _dbConnection.Query<SalesSummaryMonthly>("sp_GetMonthlySalesSummary", new { Year = year, Month = month }, commandType: CommandType.StoredProcedure);
        }
    }
}


