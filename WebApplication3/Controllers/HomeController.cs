using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication3.database;
using WebApplication3.Models;
using SimpleHashing.Net;
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
        public static WebApplication3.Models.Customer _customer =new Models.Customer();
        public static WebApplication3.Models.Account _account = new Models.Account();
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
                            Customer.Cid = c.CustomerID.ToString();
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
        public async Task<WebApplication3.Models.Customer> GetCustomerByEmailAsync(string ID)
        {
            try
            {
                // Find the customer entity by the specified email
                var customer = await _dataBaseContext.Customer
                    .FirstOrDefaultAsync(c => c.Cid == ID);

                // Return the customer entity (or null if not found)
                return customer;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }
        public async Task<WebApplication3.Models.Login> GetPassword(int ID)
        {
            try
            {
                // Find the customer entity by the specified email
                var Login = await _dataBaseContext.Login
                    .SingleOrDefaultAsync(c => c.CustomerId == ID);

                // Return the customer entity (or null if not found)
                return Login;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }
        public async Task<WebApplication3.Models.Account> GetAccount(int ID)
        {
            try
            {
                // Find the customer entity by the specified email
                var Account = await _dataBaseContext.Account
                    .FirstOrDefaultAsync(c => c.CustomerId == ID);

                // Return the customer entity (or null if not found)
                return Account;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }
        public static void setData(WebApplication3.Models.Account acc,WebApplication3.Models.Customer cus)
        {           
           _account = acc;
           _customer=cus;
        }

        public List<WebApplication3.Models.Account> GetAccountByCustomer(int id)
        {
            // Fetch customers from EF Core context based on the City column
            return _dataBaseContext.Account.Where(c => c.CustomerId == id).ToList();
        }
        [HttpPost]
        public async Task<ActionResult> Index(User user)
        {
            try
            {
                // Retrieve customer asynchronously
                var customer = await GetCustomerByEmailAsync(user.Id);

                // Check user credentials (dummy validation for this example)
                if (customer != null)
                {
                    Debug.WriteLine("cC:", customer.City);
                    var login= await GetPassword(customer.Id);
                    string passwordHash = login.PasswordHash;
                    Debug.WriteLine("cC:", login.PasswordHash);
                    if (new SimpleHash().Verify(user.Password, passwordHash))
                    {
                        //this._customer = customer;
                        var acc=await GetAccount(customer.Id);
                        Debug.WriteLine("acc:  " + acc.Id);
                        if (acc != null)
                        {
                            Debug.WriteLine("herer");
                            //this._account = acc;
                           // Debug.WriteLine("acc:  " + this._account.Id);
                        }
                        setData(acc, customer);
                        return RedirectToAction("LandingPage", "Home"); 
                    }
                    else
                    {
                        Debug.WriteLine("Invalid login attempt.");
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> WidthDraw(WidthDraw data)
        {
            try
            {
                //Debug.WriteLine("this._customer.Id ", this._customer.Cid);
                //var account = await GetAccount(this._customer.Id);
                var Transaction2 = new WebApplication3.Models.Transaction();
                //readSQL.InsertTransaction("D", acc.AccountNumber, 0, trans.Amount, trans.Comment ?? "", trans.TransactionTimeUtc ?? "");
                DateTime utcNow = DateTime.UtcNow;
                Transaction2.TransactionType = "W";
                Transaction2.AccountId = _account.Id;
                Transaction2.Comment = data.Comment;
                Transaction2.TransactionTimeUTC = utcNow.ToString();
                Transaction2.Amount = data.Cash;
                _dataBaseContext.Transaction.Add(Transaction2);
                await _dataBaseContext.SaveChangesAsync();
                return View();

            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Deposit(WidthDraw data)
        {
            try
            {
                //Debug.WriteLine("this._customer.Id ", this._customer.Cid);
                //var account = await GetAccount(this._customer.Id);
                var Transaction2 = new WebApplication3.Models.Transaction();
                //readSQL.InsertTransaction("D", acc.AccountNumber, 0, trans.Amount, trans.Comment ?? "", trans.TransactionTimeUtc ?? "");
                DateTime utcNow = DateTime.UtcNow;
                Transaction2.TransactionType = "W";
                Transaction2.AccountId = _account.Id;
                Transaction2.Comment = data.Comment;
                Transaction2.TransactionTimeUTC = utcNow.ToString();
                Transaction2.Amount = data.Cash;
                _dataBaseContext.Transaction.Add(Transaction2);
                await _dataBaseContext.SaveChangesAsync();
                return View();

            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> Transfer(Transfer data)
        {
            try
            {
                //Debug.WriteLine("this._customer.Id ", this._customer.Cid);
                //var account = await GetAccount(this._customer.Id);
                var Transaction2 = new WebApplication3.Models.Transaction();
                //readSQL.InsertTransaction("D", acc.AccountNumber, 0, trans.Amount, trans.Comment ?? "", trans.TransactionTimeUtc ?? "");
                DateTime utcNow = DateTime.UtcNow;
                Transaction2.TransactionType = "W";
                Transaction2.AccountId = _account.Id;
                Transaction2.Comment = data.Comment;
                Transaction2.TransactionTimeUTC = utcNow.ToString();
                Transaction2.Amount = data.Amount;
                Transaction2.DestinationAccountNumber = data.DestinationAccountId;
                _dataBaseContext.Transaction.Add(Transaction2);
                await _dataBaseContext.SaveChangesAsync();
                return View();

            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult LandingPage()
        {
            return View();
        }
        public IActionResult WidthDraw()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            return View();
        }
        public IActionResult Transfer()
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