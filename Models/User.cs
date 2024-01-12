namespace practice.Models;
using System.ComponentModel.DataAnnotations;

public class User 
{
    [Required ]
    // [Range(6,8,ErrorMessage ="username should contain minimum of 5 characetrs")]
    [RegularExpression("^[a-zA-z0-9]+$",ErrorMessage ="The username should not contain any special characters")]
    public string? Name{get; set;}
    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage ="Password should contain atleast one uppercase letter, one lowwercase, one special character and one number")]
    public string? password{get; set;}
}
