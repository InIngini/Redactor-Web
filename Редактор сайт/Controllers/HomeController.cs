using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Редактор_сайт.Models;
using Редактор_сайт.UseCase;

namespace Редактор_сайт.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> Logger;
		
		public HomeController(ILogger<HomeController> logger)
		{
			Logger = logger;
        }

        public IActionResult Index() // Переход на главную страничку
        {

            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (ipAddress != "::1")
            {
                string connectionString = "workstation id=InStories.mssql.somee.com;packet size=4096;user id=Ingini_SQLLogin_1;pwd=m19zvm8xz9;data source=InStories.mssql.somee.com;persist security info=False;initial catalog=InStories;TrustServerCertificate=True";
                // Создание подключения и команды
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание SQL-запроса с параметрами
                    string query = "INSERT INTO statistika (IPAddress,Time) VALUES (@IPAddress, @Time)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IPAddress", ipAddress);
                        command.Parameters.AddWithValue("@Time", (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                        // Выполнение команды вставки
                        command.ExecuteNonQuery();
                    }
                }
            }

            Text textBox = new Text();//по-другому не работает, нужен объект изначально
            return View(textBox);
        }

        public IActionResult Offormlenie(Text textBox) // теперь только один параметр
        {
            try
            {
                if (Request.Form.ContainsKey("ClearText") && !string.IsNullOrEmpty(Request.Form["ClearText"]))
                {
                    // Очистить значение Текст в модели
                    textBox.Текст = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine(ex.Message);
            }

            // Вызов метода Start с передачью выбранных опций
            if (!string.IsNullOrEmpty(textBox.Текст))
            {
                string[] paragraphs = textBox.Текст.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var data = new Data()
                {
                    text = textBox.Текст,
                    addTab = textBox.OptionT == "да", // Преобразуем строку в булевое значение
                    addParagraph = textBox.OptionP == "да" // Преобразуем строку в булевое значение
                };
                var ofform = new Ofform();
                var dataAfter = ofform.Format(data);

                var text = dataAfter.text;
                textBox.Текст_после = text;
            }
            else
            {
                textBox.Текст_после = "";
            }

            ModelState.Clear();

            return View("Index", textBox); // возвращаем индекс в папке home
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
