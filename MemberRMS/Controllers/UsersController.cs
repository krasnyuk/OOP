using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MemberRMS.Models;
using Newtonsoft.Json;

namespace MemberRMS.Controllers
{
    public class UsersController : Controller
    {
        static readonly IUserRepository repository = new UserRepository();


        private RecipeManagmentSystemEntities _db = new RecipeManagmentSystemEntities();

        static List<User> users = new List<User>();



        public dynamic _GetUsers(string sidx, string sord, int page, int rows)
        {

            var users = repository.GetAll() as IEnumerable<UserViewModel>;
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = users.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            users = users.Skip(pageIndex * pageSize).Take(pageSize);
            return new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from user in users
                    select new
                    {
                        i = user.UserID.ToString(),
                        cell = new string[] {
                         user.UserID.ToString(),
                         user.FirstName,
                         user.Telephone,
                         user.Mail.ToString()
                        }
                    }).ToArray()
            };
        }


        public void PutProduct(int id, UserViewModel item)
        {
            item.UserID = id;
            if (!repository.Update(item))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            _db.SaveChanges();
        }

        public HttpResponseMessage DeleteProduct(int id)
        {
            repository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        public static IEnumerable<UserViewModel> GetUsers(String sidx, String sord)
        {
            using (var db = new RecipeManagmentSystemEntities())
            {
                String orderBytext = String.Format("it.{0} {1}", sidx, sord);
                var result = (from p in db.User.OrderBy(x => x.FirstName)
                              select new UserViewModel
                              {
                                  UserID = p.UserID,
                                  FirstName = p.FirstName,
                                  LastName = p.LastName,
                                  Birthday = p.Birthday,
                                  
                                  Telephone = p.Telephone,
                                  Mail = p.Mail

                              }).ToList();
                return result;
            }
        }
        public JsonResult UsersData(string sidx, string sord, int page, int rows)
        {

            var list = GetUsers(sidx, sord);
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = list.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            list = (list.Skip(pageIndex * pageSize).Take(pageSize)).ToList();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from p in list
                    select new
                    {
                        i = p.UserID.ToString(),
                        cell = new string[] {
                            p.UserID.ToString(),
                            p.FirstName,
                            p.LastName,
                            
                            p.Telephone,
                            p.Mail,
                            p.Birthday.ToString(),

                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        // GET: Users
        public ActionResult Index()
        {


            return View();
        }

        [System.Web.Http.HttpPost]
        public void Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {

                User item = new User
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    
                    Telephone = user.Telephone,
                    Mail = user.Mail,
                    Birthday = user.Birthday,

                };


                _db.User.Add(item);
                _db.SaveChanges();

            }
        }
        [System.Web.Http.HttpPost]
        public void Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                User item = new User
                {
                    UserID = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    
                    Telephone = user.Telephone,
                    Mail = user.Mail,
                    Birthday = user.Birthday,
                };


                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();

            }
        }
        [System.Web.Http.HttpPost]
        public ActionResult Delete(int id)
        {
            User user = _db.User.Find(id);
            _db.User.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
