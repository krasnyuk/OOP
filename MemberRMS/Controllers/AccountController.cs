using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MemberRMS.Models;
using WebMatrix.WebData;

namespace MemberRMS.Controllers
{
    public class AccountController : Controller
    {
        //берем данные введенные в представление
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //используя полученные данные входим в систему. в зависимости от условий и прав доступа
        [HttpPost]
        [ValidateAntiForgeryToken]
        //входим в систему, сравнивая поля Логин и Пароль с существуюющими в базе
        public ActionResult Login(Login logindata, string ReturnUrl)
        {
            //если модель валидна
            if (ModelState.IsValid)
            {
                //если прошли валидацию то входим
                if (WebSecurity.Login(logindata.Username, logindata.Psssword,logindata.RememberMe))
                {
                    if (ReturnUrl!=null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                //иначе повторный вызов представления для ввода правильных данных
                else
                {
                    ModelState.AddModelError("", "Sorry the username or password is invalid");
                    return View(logindata);
                }
                
            }
            ModelState.AddModelError("", "Sorry the username or password is invalid");
            return View(logindata);
        }
        //получаем данные с представления Регистрации
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //записываем данные в базу, если введенные знаечения введены правильно
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register regdata, string role,string gender)
        {
            if (ModelState.IsValid)
            {
                //исключение на правильност ьвведенынх значений
                try
                {
                    //создаем пользователя с полученными с представления полями
                    WebSecurity.CreateUserAndAccount(regdata.Username, regdata.Password,  new
                        {
                            regdata.FirstName, regdata.LastName, regdata.Birthday, regdata.Telephone                           
                        });
                    Roles.AddUserToRole(regdata.Username, role);
                    

                    return RedirectToAction("Index", "Home");
                }
                //информируем пользователя об ошибках при неправильном вводе
                catch (MembershipCreateUserException)
                {
                    ModelState.AddModelError("", "Sorry the username is already exists");
                    return View(regdata);
                }
            }
            //ошибка при вводе, из за того, тч опользователь с таким именем существует
            ModelState.AddModelError("","Sorry the username is already exists");
            return View(regdata);
        }
        //выход из системы
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}