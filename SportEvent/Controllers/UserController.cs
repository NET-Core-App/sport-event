using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportEvent.Data.Implementations;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using Serilog;
using System.Net;
using SportEvent.Data;

namespace SportEvent.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly AuthorizationService _authorization;
        private readonly HttpClient _apiClient;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, AuthorizationService authorization)
        {
            _userRepository = userRepository;
            _logger = logger;
            _authorization = authorization;
            _apiClient = new HttpClient();
        }

        public IActionResult Unauthorized()
        {
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Profile()
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var user = await _userRepository.GetProfile(_apiClient);
                return View(user);
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }

        [HttpGet]
        public ViewResult Register() => View();


        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var registrationSuccess = await _userRepository.Register(model);

                    if (registrationSuccess)
                    {
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        var errorMessage = "Failed to register user.";
                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View(model);
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var data = await _userRepository.GetProfile(_apiClient);
                return View(data);
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }


        [HttpPost]
        public async Task<ActionResult> Edit(UserModel data)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var user = await _userRepository.Edit(data, _apiClient);

                if (user == false)
                {
                    return View(data);
                }
                return RedirectToAction("Profile", "User");
            }
            catch (HttpRequestException ex)
            {
                return View(data);
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }


        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePassword data)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var user = await _userRepository.ChangePassword(data, _apiClient);

                if (user == false)
                {
                    return View(data);
                }

                return RedirectToAction("Profile", "User");
            }
            catch (HttpRequestException ex)
            {
                return View(data);
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete()
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var eventToDelete = await _userRepository.GetProfile(_apiClient);
                return View(eventToDelete);
            }
            catch (Exception ex)
            {
                throw; // Rethrow the exception for it to be handled by the built-in exception handling.
            }
        }


        [HttpPost]
        public async Task<ActionResult> Delete(IFormCollection collection)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var eventToDelete = await _userRepository.Delete(_apiClient);
               
                if (eventToDelete)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [HttpGet]
        public ViewResult Login() => View();


        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isAuthenticated = await _userRepository.AuthenticateUser(model);

                    if (isAuthenticated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong Email or Password"); 
                        return View(model);
                    }
                }

         
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                // Clear the session
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Logout action.");
                return RedirectToAction("Index", "Home"); // Or handle the error in a way that makes sense for your application
            }
        }

    }
}
