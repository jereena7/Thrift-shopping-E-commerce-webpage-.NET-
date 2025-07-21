using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThriftShop.DataAccessLayer;
using ThriftShop.Models;

namespace ThriftShop.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserDAL _userDAL;




        public AccountController(UserDAL userDAL)
        {
            //// Replace with your actual connection string
            //string connectionString = "YourConnectionStringHere";
            //_userDAL = new UserDAL(connectionString);
            _userDAL = userDAL;

        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectBasedOnRole(HttpContext.Session.GetString("UserRole"));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userDAL.ValidateUser(username, password);

            if (user != null)
            {
                // Store user details in session
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserRole", user.UserRole); // Ensure UserRole is set

                // Create claims for the user
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.UserRole) // Ensure UserRole is added as a claim
        };

                // Create identity and principal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Redirect based on user role
                return RedirectBasedOnRole(user.UserRole);
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        private IActionResult RedirectBasedOnRole(string userRole)
        {
            if (userRole == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (userRole == "User")
            {
                return RedirectToAction("Index", "Shopping");
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please correct the errors and try again.";
                return View(model);
            }

            try
            {
                // Register the user
                _userDAL.RegisterUser(model);

                // Redirect to login page after successful registration
                ViewBag.SuccessMessage = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred during registration. Please try again.";
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                // Generate reset token
                string resetToken = _userDAL.GenerateResetToken(email);

                // Simulate sending an email with the reset link
                string resetLink = Url.Action("ResetPassword", "Account", new { token = resetToken }, Request.Scheme);
                Console.WriteLine($"Reset Link: {resetLink}"); // Replace with actual email sending logic

                ViewBag.SuccessMessage = "A password reset link has been sent to your email.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            ViewBag.Token = token; // Pass the token to the view
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            try
            {
                // Reset the password
                _userDAL.ResetPassword(token, newPassword);

                ViewBag.SuccessMessage = "Your password has been reset successfully.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Invalid or expired reset token.";
                return View();
            }
        }

        public void TestMethod()
        {
            // Test method for debugging
        }
    }
}