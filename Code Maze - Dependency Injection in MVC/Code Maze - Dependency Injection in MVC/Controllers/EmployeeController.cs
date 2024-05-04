using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Code_Maze___Dependency_Injection_in_MVC.Controllers;

public class EmployeeController : Controller
{
    private readonly IDataRepository<Employee> _dataRepository;

    public EmployeeController(IDataRepository<Employee> DataRepository)
    {
        _dataRepository = DataRepository;
    }

    //public IActionResult index()
    //{
    //    IEnumerable<Employee> employees = _dataRepository.GetAll();

    //    return View(employees);
    //}

    public IActionResult Index([FromServices] IDataRepository<Employee> _dataRepository)
    {
        IEnumerable<Employee> employees = _dataRepository.GetAll();
        return View(employees);
    }
}