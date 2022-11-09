namespace Demo1_ASP_MVC.Models
{
    public class User
    {
       

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }

        public string Password { get; set; }

   
    }
}
