#nullable disable
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class User : IdentityUser
    {
        [Required]
        [PersonalData]
        [Display(Name = "Nazwa użytkownika")]
        public string NickName { get; set; }
    }
}
