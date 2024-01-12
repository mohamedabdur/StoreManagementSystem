using System.ComponentModel.DataAnnotations;
namespace practice.Models;

public class NewUser
{
    [Required]
    [RegularExpression("^[a-zA-z0-9]+$",ErrorMessage ="The username should not contain any special characters or numbers")]
    public string? Name{get; set;}
    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage ="Password should contain atleast one uppercase letter, one lowwercase, one special character and one number")]
    public string? password{get; set;}
    [Required][Compare("password",ErrorMessage ="Password Dosent Match")][Display(Name ="Confirm Password")]
    public string? reenteredpassword{get; set;}    
    [Required]
    [Range(1,7,ErrorMessage ="The number should be within 1 to 7")]
    [StringLength(2,MinimumLength=1,ErrorMessage ="The digit should be 1 or 2.")]
    public string? floor{get; set;}   
}
