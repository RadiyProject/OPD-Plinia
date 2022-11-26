using System.ComponentModel.DataAnnotations;

namespace Plinia_AuthService.Models;

public class Credentials
{
    public Credentials(string email, string password, string username)
    {
        Email = email;
        Password = password;
        Username = username;
    }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { set; get; }
    
    [Required]
    [StringLength(100)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { set; get; }
    
    [Required]
    [StringLength(100)]
    [Display(Name = "Username")]
    public string Username { set; get; }
}