using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Редактор_сайт.Models;

namespace Редактор_сайт.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
            //isFirstUserVisit = true;
        }

        private static bool isFirstUserVisit = true;
        private static readonly object lockObject = new object();
        public IActionResult Index()//переход на главную страничку
        {

            // Проверяем, был ли уже первый визит пользователя
            //if (isFirstUserVisit)
            {
                //lock (lockObject)
                {
                    //if (isFirstUserVisit)
                    {
                        string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                        // Ваш код, который должен выполниться только один раз
                        // Получение строки подключения из appsettings.json
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
                        isFirstUserVisit = false;
                    }
                }
            }


            Text textBox = new Text();//по-другому не работает, нужен объект изначально
            return View(textBox);
        }
        
		//это чтобы нельзя было перейти по прямому адресу
        public IActionResult Offormlenie(Text textBox, string option)//содержит инфу от пользователя
        {
            try
            {
                if (Request.Form.ContainsKey("ClearText") && !string.IsNullOrEmpty(Request.Form["ClearText"]))
                {
                    // Очистить значение Текст в модели
                    textBox.Текст = string.Empty;
                }
            }
            catch (Exception ex) { }

            // Вызов метода Start с передачей выбранных опций
            if (!string.IsNullOrEmpty(textBox.Текст) && textBox.Текст != null)
            {
                string[] paragraphs = textBox.Текст.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                string text = Ofform.Start(paragraphs);
                Ofform.index = 0;

                // Проверка выбранного значения радиокнопки
                if (option == "нет")
                {
                    text = text.Replace("<tab>", "").Replace("<center>", "").Replace("</center>", "\n");
                }
                else
                {
                    text = text.Replace("\n\n", "\n  \n");
                }

                textBox.Текст_после = text;
            }
            else
            {
                textBox.Текст_после = "";
            }
            ModelState.Clear();

            return View("Index",textBox);//возвращаем индекс в папке home
        }
       
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
