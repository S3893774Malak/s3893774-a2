using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication3.database;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class Transaction
    {
        public double Amount { get; set; }
        public string? Comment { get; set; }
        public string? TransactionTimeUtc { get; set; }
    }
    public class Account
    {
        public required int AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public int CustomerID { get; set; }
        public Transaction[]? Transactions { get; set; }
    }
    public class LoginCredentials
    {
        public required string? LoginID { get; set; }
        public string? PasswordHash { get; set; }
    }
    public class Customer
    {
        public required int CustomerID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostCode { get; set; }
        public Account[]? Accounts { get; set; }
        public LoginCredentials? Login { get; set; }

    }
    public class HomeController : Controller
    {
        private readonly DataBaseContext _dataBaseContext;

        public HomeController(DataBaseContext context)
        {
            _dataBaseContext = context;
        }
        public async Task loadDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make a GET request to the REST API
                    HttpResponseMessage response = await client.GetAsync("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");

                    // Check if the request was successful
                    
                    
                    
                  
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string json = await response.Content.ReadAsStringAsync();
                        Customer[] customer = JsonConvert.DeserializeObject<Customer[]>(json);
                        foreach (Customer c in customer)
                        {
                            //readSQL.InsertCustomer(c.CustomerID, c.Name ?? "", c.Address ?? "", c.City ?? "", c.PostCode ?? "");
                            var Customer = new WebApplication3.Models.Customer();
                            Customer.Name = c.Name;
                            Customer.City = c.City;
                            Customer.Address = c.Address;
                            Customer.Postcode = c.PostCode;
                            _dataBaseContext.Customer.Add(Customer);
                            await _dataBaseContext.SaveChangesAsync();
                            var insertedPrimaryKeyValue = Customer.Id;
                            //readSQL.InsertLogin(c.Login.LoginID, c.CustomerID, c.Login.PasswordHash);
                            var Login = new WebApplication3.Models.Login();
                            
                            Login.CustomerId = insertedPrimaryKeyValue;
                            Login.PasswordHash = c.Login.PasswordHash;
                            _dataBaseContext.Login.Add(Login);
                            await _dataBaseContext.SaveChangesAsync();
                            foreach (Account acc in c.Accounts)
                            {
                                var Account = new WebApplication3.Models.Account();
                                double balance = 0;
                                //readSQL.InsertAccount(acc.AccountNumber, acc.AccountType, c.CustomerID, balance);
                                Account.AccountType = acc.AccountType;
                                Account.CustomerId = insertedPrimaryKeyValue;
                                _dataBaseContext.Account.Add(Account);
                                await _dataBaseContext.SaveChangesAsync();
                                var insertedPrimaryKeyValue2 = Account.Id;
                                foreach (Transaction trans in acc.Transactions)
                                {
                                    var Transaction = new WebApplication3.Models.Transaction();
                                    balance += trans.Amount;
                                    //readSQL.InsertTransaction("D", acc.AccountNumber, 0, trans.Amount, trans.Comment ?? "", trans.TransactionTimeUtc ?? "");
                                    Transaction.TransactionType = "D";
                                    Transaction.AccountId = insertedPrimaryKeyValue2;
                                    Transaction.Comment = trans.Comment;
                                    Transaction.TransactionTimeUTC = trans.TransactionTimeUtc;
                                    Transaction.Amount = (int)trans.Amount;
                                    _dataBaseContext.Transaction.Add(Transaction);
                                    await _dataBaseContext.SaveChangesAsync();
                                }
                                //readSQL.UpdateSingleColumn("Balance", "Account", "AccountNumber", balance, acc.AccountNumber);


                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("The request was not successful. Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        public async Task<bool> CheckIfAnyDataExistsAsync()
        {
            // Perform a query to check if any records exist in the table
            var anyDataExists = await _dataBaseContext.Customer.AnyAsync();

            // 'anyDataExists' will be 'true' if there's any data in the 'Login' table, 'false' otherwise
            return anyDataExists;
        }
        public async Task<IActionResult> Index()
        {   
            if(!await CheckIfAnyDataExistsAsync())
            await loadDataAsync();
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            // Check user credentials (dummy validation for this example)
            if (user.Id == "admin" && user.Password == "password")
            {
                return RedirectToAction("Privacy", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}