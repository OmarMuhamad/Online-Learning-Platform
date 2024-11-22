using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data;
public class TokenRequestModel {
    
    [Required , StringLength(50)]
    public string Email { get; set; }
    
    [Required , StringLength(256)]
    public string Password { get; set; }
    
}