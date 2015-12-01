using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MemberRMS.Models
{
    public class Register
    {
        [Display(Name = "Имя пользователя*")]
        [Required(ErrorMessage = "Необходимо ввести имя пользователя или адресс электронной почты")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Электронная почта введена неверно")]
        public string Username { get; set; }

        [StringLength(100, ErrorMessage = "Пароль должен содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        [Required(ErrorMessage = "Необходимо ввести пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля*")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]

        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Необходимо ввести имя")]
        [Display(Name = "Имя*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        [Display(Name = "Фамилия*")]
        public string LastName { get; set; }
        [Display(Name = "Дата рождения")]
        public string Birthday { get; set; }
        [Display(Name = "Номер телефона")]
        public string Telephone { get; set; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }


    }
}