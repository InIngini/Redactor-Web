using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Редактор_сайт.Models
{
    public class Text
    {
        [Display(Name = "Введите текст")]
        public string Текст { get; set; }

        [Display(Name = "Оформленный текст")]
        public string Текст_после { get; set; }

        [Display(Name = "Добавлять ли &lt;tab&gt; и &lt;center&gt;?")]
        public string OptionT { get; set; } // Для первой группы радиокнопок

        [Display(Name = "Добавлять ли абзац между диалогом и текстом?")]
        public string OptionP { get; set; } // Для второй группы радиокнопок
    }
}
