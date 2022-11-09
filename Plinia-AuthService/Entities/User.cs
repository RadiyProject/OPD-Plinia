using System.ComponentModel.DataAnnotations.Schema;

namespace Plinia_AuthService.Entities;

[Table("users")]
public class User
{
    public int Id { set; get; }

    public string Username { set; get; }

    public string Password { set; get; }
}