using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Редактор_сайт.Models
{
    public class Text
    {
        [Display(Name = "Введите текст")]
        //[MinLength(2, ErrorMessage = "Пусто... У вас творческий кризис?")]
        //[Required(ErrorMessage = "Поле 'Введите текст' должно быть заполнено")]
        public string Текст { get; set; }

        [Display(Name = "Оформленный текст")]
        public string Текст_после { get; set; }
    }
}
