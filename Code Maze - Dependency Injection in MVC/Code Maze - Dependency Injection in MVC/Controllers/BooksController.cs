using Microsoft.AspNetCore.Mvc;

namespace Code_Maze___Dependency_Injection_in_MVC.Controllers;

public class BooksController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}