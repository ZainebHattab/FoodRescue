using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodRescue.web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminTestController : Controller
{
    public IActionResult Index() => View();
}