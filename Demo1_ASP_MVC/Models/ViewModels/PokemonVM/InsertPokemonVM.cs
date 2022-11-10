using System.ComponentModel.DataAnnotations;

namespace Demo1_ASP_MVC.Models.ViewModels.PokemonVM
{
    public class InsertPokemonVM
    {
        public int Id { get; set; }
       
        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [MinLength(2, ErrorMessage = "Le prénom doit etre minimum de 2 carateres")]
        [MaxLength(24, ErrorMessage = "Le prénom doit etre maximum de 24 carateres")]
        public string? FrenchName { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [MinLength(2, ErrorMessage = "Le nom doit etre minimum de 2 carateres")]
        [MaxLength(24, ErrorMessage = "Le nom doit etre maximum de 24 carateres")]
        public string? EnglishName { get; set; }

        public string? Image { get; set; }


    }
}
