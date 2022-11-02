using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.Controllers
{
    public class UserManagementController : Controller
    {

        private readonly ApplicationDbContext _db;
      

        public UserManagementController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Create_Account")]
        public IActionResult Index()
        {
            return View();
        }



        // Post Method retrieve data from create account page
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create_Account")]
        public IActionResult Index(User obj)
        {
            try
            {
                // If user is found in the database return back to create account page
                if (_db.Users.Any(U => U.UserName == obj.UserName))
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                    return View();
                }
                // user is not found
                else
                {
                    var salt = DateTime.Now.ToString();
                    var HasedPW = HashPassword($"{obj.Password}{salt}");

                    _db.Users.Add(new User() { UserName = obj.UserName, Password = HasedPW, Salt = salt });
                    _db.SaveChanges();

                }

                return RedirectToAction("Index", "Home");
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Home");

            }
        }

        private string HashPassword(string password)
        {
            SHA256 hash = SHA256.Create();

            var PasswordBytes = Encoding.Default.GetBytes(password); // Change password to bytes

            var HashedPassword = hash.ComputeHash(PasswordBytes);


            return Convert.ToHexString(HashedPassword);



        }
        [Route("Login")]
        public IActionResult Login()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]


        public async Task<IActionResult> Login(User obj)
        {
            try
            {
                // If username is not found in the database
                if (!(_db.Users.Any(U => U.UserName == obj.UserName)))
                {
                    ModelState.AddModelError("UserName", "Username does not exist.");
                    return View();
                }

                // If username exists and password is incorrect
                else
                {
                    var row = _db.Users.Where(U => U.UserName == obj.UserName).ToList(); // Grab the row from the database where the user is in
                    string InputPassword = obj.Password;
                    var Salt = row[0].Salt; // Retrieve Salt from row

                    var HashedPassword = HashPassword($"{InputPassword}{Salt}");

                    //If the used input password matches the password stored in password
                    if (row[0].Password == HashedPassword)
                    {

                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, obj.UserName),
                        new Claim(ClaimTypes.Role,"user"),
                    };

                        var indentity = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(indentity);
                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect password.");
                        return View();
                    }

                }
            }
            catch(Exception e)
            {
                return View("Error", e);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login","UserManagement");
        }
    }

  


}
