using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using W04_Phase_01.DAL;
using W04_Phase_01.Models;

namespace W04_Phase_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly DALPerson _dalPerson;

        public HomeController(IConfiguration config)
        {
            _dalPerson = new DALPerson(config);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int newPersonId = _dalPerson.InsertPerson(person);
                    HttpContext.Session.SetInt32("PersonId", newPersonId);

                    return View("Page2", person); // Show confirmation
                }
                catch (SqlException ex) when (ex.Number == 2627)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                }
            }
            return View(person);
        }

        public IActionResult Edit()
        {
            int? personId = HttpContext.Session.GetInt32("PersonId");
            if (personId == null)
            {
                return RedirectToAction("Index");
            }

            Person person = _dalPerson.GetPersonById(personId.Value);
            return View(person);
        }

        [HttpPost]
        public IActionResult Edit(Person person)
        {
            // No DB update in this phase — just redisplay edited info
            return View("Page2", person);
        }

        public IActionResult ViewData()
        {
            var persons = _dalPerson.GetAllPersons();
            return View(persons);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
