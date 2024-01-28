using System.ComponentModel.DataAnnotations;

namespace Редактор_сайт.Models
{
    public class Contact
    {
        [Display(Name="Введите имя")]
        [Required(ErrorMessage ="Вам нужно ввести имя")]
        [StringLength(30, ErrorMessage = "Менее 30 символов, пожалуйста")]
        public string Name { get; set; }

        [Display(Name = "Введите email")]
        [Required(ErrorMessage = "Вам нужно ввести email")]
        public string Email { get; set; }

        [Display(Name = "Введите сообщение")]
        [Required(ErrorMessage = "Вам нужно ввести сообщение")]
        public string Message { get; set; }
    }
}
