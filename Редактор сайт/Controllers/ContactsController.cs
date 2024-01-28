using Microsoft.AspNetCore.Mvc;
using Редактор_сайт.Models;

namespace Редактор_сайт.Controllers
{
	public class ContactsController : Controller
	{
		public IActionResult Index()//будет сразу срабатывать, когда мы перейдем на contacts
		{
			return View();
		}

		[HttpPost]//это чтобы нельзя было перейти по прямому адресу
        public IActionResult Check(Contact contact)//содержит инфу от пользователя
        {
			if(ModelState.IsValid)//корректны ли данные
			{
				return Redirect("/");//переадрессация на главную страницу
			}

            return View("Index");//возвращаем индекс в папке контактс
        }
    }
}
