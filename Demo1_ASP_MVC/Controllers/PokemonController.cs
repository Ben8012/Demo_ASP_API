using Demo1_ASP_MVC.Models;
using Demo1_ASP_MVC.Models.PokemonModels;
using Demo1_ASP_MVC.Models.ViewModels.PokemonVM;
using Demo1_ASP_MVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo1_ASP_MVC.Controllers
{
    public class PokemonController : Controller
    {

        private readonly string BASEAPI = "https://localhost:7264/api/"; 

        private readonly SessionManager _sessionManager;


        public PokemonController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;

        }

        // GET: PokemonController
        public ActionResult Index()
        {
            return RedirectToAction(nameof(ListPokemon));
        }

        public async Task<ActionResult> ListPokemon()
        {
            List<Pokemon> pokemons = new List<Pokemon>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.GetAsync("Pokemon/GetAll");
            if (reponse.IsSuccessStatusCode)
            {
                pokemons = await reponse.Content.ReadFromJsonAsync<List<Pokemon>>();
            }
            else
            {
                ErrorResponse? errorResponse = await reponse.Content.ReadFromJsonAsync<ErrorResponse>();
                ModelState.AddModelError("contact", errorResponse.Message);
            }

            ListPokemonVM listPokemons = new ListPokemonVM(pokemons.OrderBy(c => c.Id).ToList());

            return View(listPokemons);
        }

   
        public ActionResult Insert()
        {
            return View();
        }

    
        [HttpPost]
        public async Task <ActionResult> Insert(InsertPokemonVM form)
        {
            if (!ModelState.IsValid)
                return View(form);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.PostAsJsonAsync("Pokemon/Insert", form);
            if (reponse.IsSuccessStatusCode)
            {
                Pokemon result = await reponse.Content.ReadFromJsonAsync<Pokemon>();
            }
            return RedirectToAction(nameof(ListPokemon));
        }

        public async Task<ActionResult> Update(int id)
        {

            InsertPokemonVM pokemon = new InsertPokemonVM();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.GetAsync($"Pokemon/GetById/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                pokemon = await reponse.Content.ReadFromJsonAsync<InsertPokemonVM>();
            }
            else
            {
                ErrorResponse? errorResponse = await reponse.Content.ReadFromJsonAsync<ErrorResponse>();
                ModelState.AddModelError("pokemon", errorResponse.Message);
            }
            return View(pokemon);
        }


        [HttpPost]
        public async Task<ActionResult> Update(InsertPokemonVM form)
        {
            if (!ModelState.IsValid) return View(form);


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.PutAsJsonAsync($"Pokemon/Update/{form.Id}", form);
            if (reponse.IsSuccessStatusCode)
            {
                int result = await reponse.Content.ReadFromJsonAsync<int>();

            }

            return RedirectToAction(nameof(ListPokemon));
        }

  
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASEAPI);
            var reponse = await client.DeleteAsync($"Pokemon/Delete/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                int nbRows = await reponse.Content.ReadFromJsonAsync<int>();
            }

            return RedirectToAction(nameof(ListPokemon));
        }

        
    }
}
