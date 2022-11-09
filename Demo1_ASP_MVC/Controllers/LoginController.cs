using Demo1_ASP_MVC.Context;
using Demo1_ASP_MVC.Models;
using Demo1_ASP_MVC.Models.ViewModels;
using Demo1_ASP_MVC.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Demo1_ASP_MVC.Controllers
{
   


    public class LoginController : Controller
    {
        private readonly string BASEAPI = "http://localhost:5201/api/"; //"https://localhost:7201/api/";

        private readonly SessionManager _sessionManager;

        public LoginController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }


        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
           
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthLoginFormVM form)
        {

            User user = new User();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.PostAsJsonAsync("Login",form);
            if (reponse.IsSuccessStatusCode)
            {
                user = await reponse.Content.ReadFromJsonAsync<User>();
            }
            else
            {
                string error = await reponse.Content.ReadAsStringAsync();
                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(error);
                ModelState.AddModelError(nameof(form.Email), errorResponse.Message);
            }
           

            //ValidatePassword(form);
            
            if (!ModelState.IsValid)
                return View();

            _sessionManager.Id = user.Id;
            _sessionManager.Email = user.Email;
            _sessionManager.FirstName = user.FirstName;
            _sessionManager.LastName = user.LastName;
            _sessionManager.Birtdate = user.Birthdate.ToLongDateString();

            return RedirectToAction("Index", "Home");

        }

        public IActionResult Logout()
        {
            _sessionManager.Clear();
            return RedirectToAction("Index", "Home");
        }

        private void ValidatePassword(AuthLoginFormVM form)
        {
            string password = form.Password;
            string min_pattern = @"[a-z]+";
            string maj_pattern = @"[A-Z]+";
            string numb_pattern = @"[0-9]+";
            string symb_pattern = @"[@\-+=#_]+";
            if (!Regex.IsMatch(password, min_pattern, RegexOptions.None))
                ModelState.AddModelError(nameof(form.Password), "Le mot de passe ne contient pas de minuscule.");
            if (!Regex.IsMatch(password, maj_pattern, RegexOptions.None))
                ModelState.AddModelError(nameof(form.Password), "Le mot de passe ne contient pas de majuscule.");
            if (!Regex.IsMatch(password, numb_pattern, RegexOptions.None))
                ModelState.AddModelError(nameof(form.Password), "Le mot de passe ne contient pas de chiffre.");
            if (!Regex.IsMatch(password, symb_pattern, RegexOptions.None))
                ModelState.AddModelError(nameof(form.Password), "Le mot de passe ne contient pas de symbole.");
        }
    }
}
