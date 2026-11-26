using Microsoft.AspNetCore.Identity;

namespace FoodRescue.web.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
    public string? City { get; set; }
}