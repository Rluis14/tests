using Microsoft.AspNetCore.Mvc;
using W04_Phase_01.Models;

public class UserController : Controller
{
	public ActionResult Index()
	{
		return View();
	}

	[HttpPost]
	public ActionResult Index(UserInfo user)
	{
		return View("Page2", user);
	}

	public ActionResult Page2(UserInfo user)
	{
		return View(user);
	}
}