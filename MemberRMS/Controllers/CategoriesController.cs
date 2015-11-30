using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MemberRMS.Models;

namespace MemberRMS.Controllers
{
    public class CategoriesController : Controller
    {
        public RecipeManagmentSystemEntities db = new RecipeManagmentSystemEntities();
        private readonly CategoriesRepository _categoriesRepository;

        public CategoriesController()
        {
            _categoriesRepository = new CategoriesRepository(this);
        }


        [HttpPost]
        public ActionResult Rename(/*[Bind(Include = "CategoryID,Title,ParentCategoryID")]*/Category node)
        {
            var a = node.ParentCategoryID;
            _categoriesRepository.SetNameById(node.CategoryID, node.Title);
            return null;
        }

        [HttpPost]
        public ActionResult Remove(Category node)
        {
            _categoriesRepository.RemoveById(node.CategoryID);
            return null;
        }

        [HttpPost]
        public ActionResult MoveNode(Category node)
        {
            _categoriesRepository.Move(node);
            return null;
        }

        [HttpPost]
        public JsonResult AddNode(Category node)
        {
            _categoriesRepository._AddNode(node);
            var id = _categoriesRepository.GetIdBy(node);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChildren(int id)
        {
            var children = _categoriesRepository.GetChildrenNodes(id);
            return Json(children, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories
        public ActionResult Index()
        {
            var nodes = _categoriesRepository.GetCategories();
            
            return View(nodes);
        }

       

        public static IEnumerable<CategoriesViewModel> GetCategories(String sidx, String sord)
        {

            using (var db = new RecipeManagmentSystemEntities())
            {
                String orderBytext = string.Format("it.{0} {1}", sidx, sord);
                var result = (from p in db.Category.OrderBy(x => x.CategoryID)
                              select new CategoriesViewModel
                              {
                                  CategoryID = p.CategoryID,
                                  ParentCategoryID = p.ParentCategoryID,
                                  Title = p.Title
                                  }).ToList();
                return result;
            }
        }

        public JsonResult CategoriesData(string sidx, string sord, int page, int rows)
        {

            var list = GetCategories(sidx, sord);
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = list.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);
            list = (list.Skip(pageIndex * pageSize).Take(pageSize)).ToList();
            var jsonData = new
            {
                total = totalPages, page,
                records = totalRecords,
                rows = (
                    from p in list
                    select new
                    {
                        i = p.CategoryID.ToString(),
                        cell = new[] {
                            
                            p.CategoryID.ToString(),
                            p.ParentCategoryID.ToString(),
                            p.Title
                     }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(Category category)
        {
            category.ParentCategoryID = 0;
            category.CategoryID  = (db.Category.Max(x => x.CategoryID) + 1) ;
            var a = category.Title;

            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult _CategoriesPartial1(Category category)
        //{
        //    var a = category;
        //    return PartialView();
        //}

        // GET: Categories/Details/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "CategoryID,Title,ParentCategoryID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

       



        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
