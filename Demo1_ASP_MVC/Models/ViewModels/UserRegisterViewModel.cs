using System.ComponentModel.DataAnnotations;

namespace Demo1_ASP_MVC.Models.ViewModels
{
    public class UserRegisterViewModel
    {

      

        [Display(Name = "Prénom")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        public string FirstName { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        public string LastName { get; set; }

        [Display(Name = "Date de naissance")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Adresse Email")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [EmailAddress(ErrorMessage = "Adresse Email Invalide")]
        public string Email { get; set; }

        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Les deux mot de passe doivent correspondre")]
        [Display(Name = "Confirmation de mot de passe")]
        [Required(ErrorMessage = "Le Champ est requis.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
