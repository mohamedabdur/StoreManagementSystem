using System.ComponentModel.DataAnnotations;

namespace practice.Models;

public class Product 
{
    [Required]
    [RegularExpression("^[a-zA-z-/.]+$",ErrorMessage ="The Product Name should be in the product name followed by Brand name with the hiphen")]
    public string? productname{get; set;}
    [Key]
    [ScaffoldColumn(false)]
    public int productid{get; set;}
    [Required]
    [RegularExpression("^[a-zA-z]+$",ErrorMessage="The Category should contain only alphabets")]
    public string? productcategory{get; set;}
    [Required]
    [RegularExpression("^[a-zA-z]+$",ErrorMessage="The Supplier Name should contain only alphabets")]
    public string? suppliername{get; set;}
    [Required]
    [RegularExpression("^[6-9]{1}[0-9]{9}",ErrorMessage="Enter a Valid Phone Number")]
    [DataType(DataType.PhoneNumber,ErrorMessage="Enter a Valid Phone Number")]
    public string? supplierPhoneNumber{get; set;}
    
    [Required]
    [DataType(DataType.Date)]
    public string? recieveddate{get; set;}
    [Required]
    public int quantity{get;set;}
}   