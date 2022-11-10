using Demo1_ASP_MVC.Models.PokemonModels;

namespace Demo1_ASP_MVC.Models.ViewModels.PokemonVM
{
    public class ListPokemonVM
    {
        public ListPokemonVM(List<Pokemon> pokemons)
        {
            _pokemons = pokemons;

        }

        private List<Pokemon> _pokemons;

        public List<Pokemon> Pokemons { get => _pokemons; }

        public int NbPokemons { get => _pokemons.Count(); }

        public void AddContat(Pokemon newPokemon)
        {
            if (newPokemon == null) throw new ArgumentNullException(nameof(newPokemon));
            if (_pokemons == null) _pokemons = new List<Pokemon>();
            if (!_pokemons.Contains(newPokemon)) _pokemons.Add(newPokemon);

        }
    }
}
