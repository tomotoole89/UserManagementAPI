namespace UserManagementAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User 
    {
        public int Id { get; set; }
    

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }


}
