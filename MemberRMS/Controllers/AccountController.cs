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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login logindata, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(logindata.Username, logindata.Psssword))
                {
                    if (ReturnUrl!=null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sorry the username or password is invalid");
                    return View(logindata);
                }
                
            }
            ModelState.AddModelError("", "Sorry the username or password is invalid");
            return View(logindata);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register regdata, string role,string gender)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(regdata.Username, regdata.Password,
                        propertyValues: new
                        {
                            FirstName = regdata.FirstName,
                            LastName = regdata.LastName,
                            Birthday = regdata.Birthday,
                            Telephone = regdata.Telephone,
                            Gender = regdata.Gender
                        });
                    Roles.AddUserToRole(regdata.Username, role);
                    

                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException)
                {
                    ModelState.AddModelError("", "Sorry the username is already exists");
                    return View(regdata);
                }
            }
            ModelState.AddModelError("","Sorry the username is already exists");
            return View(regdata);
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}