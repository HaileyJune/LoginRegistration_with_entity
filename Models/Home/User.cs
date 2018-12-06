using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    

namespace LoginRegistration.Models
{
public class UserObject
{
    [Key]
    public int UserId {get;set;}
    [Required]
    [MinLength(2)]
    [Display (Name = "Your First Name")]
    public string FirstName {get;set;}
    [Required]
    [MinLength(2)]

    public string LastName {get;set;}
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="This Password is Sub-Par. Try again.")]
    public string Password {get;set;}
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
}    
}