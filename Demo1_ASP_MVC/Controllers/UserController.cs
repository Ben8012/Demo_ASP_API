using Demo1_ASP_MVC.Models;
using Demo1_ASP_MVC.Models.ViewModels;
using Demo1_ASP_MVC.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Demo1_ASP_MVC.Controllers
{
    public class UserController : Controller
    {

        private readonly string BASEAPI = "http://localhost:5201/api/"; //"https://localhost:7201/api/";

        private readonly SessionManager _sessionManager;


        public UserController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            
            return View();
        }
            

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel form)
        {


            if (!ModelState.IsValid)
                return View();

            User user = new User();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.PostAsJsonAsync("User/Insert", form);
            if (reponse.IsSuccessStatusCode) user = await reponse.Content.ReadFromJsonAsync<User>();

            if (user == null)
                ModelState.AddModelError(nameof(form.Email), "l'utilisateur n'existe pas");

            ValidatePassword(form);

            if (!ModelState.IsValid)
                return View();

            AddUserToSession(user);

            return RedirectToAction("Index", "Home");

        }



        public async Task<IActionResult> Update()
        {
            UserRegisterViewModel user = new UserRegisterViewModel();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.GetAsync($"User/GetById/{_sessionManager.Id}");
            if (reponse.IsSuccessStatusCode)
            {
                user = await reponse.Content.ReadFromJsonAsync<UserRegisterViewModel>();

            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserRegisterViewModel form)
        {
            User user = new User();

            var userToUpdate = new
            {
                LastName = form.LastName,
                FirstName = form.FirstName,
                Email = form.Email,
                Birthdate = form.Birthdate,
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.PutAsJsonAsync($"User/Update/{(int)_sessionManager.Id}", userToUpdate);
            if (reponse.IsSuccessStatusCode)
            {
                user = await reponse.Content.ReadFromJsonAsync<User>();

            }

            AddUserToSession(user);

            return RedirectToAction("Index", "Home");
        }


        private void AddUserToSession(User user)
        {
            _sessionManager.Id = user.Id;
            _sessionManager.Email = user.Email;
            _sessionManager.FirstName = user.FirstName;
            _sessionManager.LastName = user.LastName;
            _sessionManager.Birtdate = user.Birthdate.ToLongDateString();
        }

        private void ValidatePassword(UserRegisterViewModel form)
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
