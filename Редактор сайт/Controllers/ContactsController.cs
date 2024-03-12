using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                // Получение строки подключения из appsettings.json
                string connectionString = "workstation id=InStories.mssql.somee.com;packet size=4096;user id=Ingini_SQLLogin_1;pwd=m19zvm8xz9;data source=InStories.mssql.somee.com;persist security info=False;initial catalog=InStories;TrustServerCertificate=True";
                // Создание подключения и команды
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание SQL-запроса с параметрами
                    string query = "INSERT INTO Contacts (Name, Email, Message) VALUES (@Name, @Email, @Message)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", contact.Name);
                        command.Parameters.AddWithValue("@Email", contact.Email);
                        command.Parameters.AddWithValue("@Message", contact.Message);

                        // Выполнение команды вставки
                        command.ExecuteNonQuery();
                    }
                }
                return Redirect("/");//переадрессация на главную страницу
			}

            return View("Index");//возвращаем индекс в папке контактс
        }
    }
}
