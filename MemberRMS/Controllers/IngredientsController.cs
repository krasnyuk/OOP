using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MemberRMS.Models;
using Microsoft.Ajax.Utilities;

namespace MemberRMS.Controllers
{
    public class IngredientsController : Controller
    {
        private RecipeManagmentSystemEntities db = new RecipeManagmentSystemEntities();
        // GET: Ingredients
        public ActionResult Index()
        {
            var ingredient = db.Ingredient.Include(i => i.Category);
            return View(ingredient.ToList());
        }
        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = db.Ingredient.Find(id);
            db.Ingredient.Remove(ingredient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult IngredientJsonResultData(string sidx, string sord, int page, int rows)
        {
            List<int> aas = new List<int>();
           
            var list = GetIngredients(sidx, sord);
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
                        i = p.IngredientID.ToString(),
                        cell = new string[] {
                            p.IngredientID.ToString(),
                            p.Title,
                            p.LongDescription,
                            p.Cost.ToString(),
                            p.Weight.ToString(),
                            p.CategoryName
                        }
                    }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public static IEnumerable<IngredientsViewModel> GetIngredients(String sidx, String sord)
        {
            using (var db = new RecipeManagmentSystemEntities())
            {
                String orderBytext = String.Format("it.{0} {1}", sidx, sord);
                var result = (from p in db.Ingredient.OrderBy(x => x.Title)
                              select new IngredientsViewModel
                              {
                                  IngredientID = p.IngredientID,
                                  Title = p.Title,
                                  LongDescription = p.LongDescription,
                                  Cost = p.Cost,
                                  Weight = p.Weight,
                                  CategoryID = p.CategoryID,
                                  CategoryName = p.Category.Title


                              }).ToList();
                return result;
            }
        }

        [System.Web.Http.HttpPost]
        public void Create([Bind(Include = "IngredientID,Title,LongDescription,Cost,Weight,CategoryID")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                // проверить на исключение
                db.Ingredient.Add(ingredient);
                db.SaveChanges();

            }
            // действия по добавлению
        }

        [System.Web.Http.HttpPost]
        public void Edit([Bind(Include = "IngredientID,Title,LongDescription,Cost,Weight,CategoryID")] Ingredient ingredient) //UserID', 'FirsName', 'LastName', 'Mail
        {
            if (ModelState.IsValid)
            {

                db.Entry(ingredient).State = EntityState.Modified;
                db.SaveChanges();

            }
            // ViewBag.RoleID = new SelectList(db.Ingredient, "", "Title", user.RoleID);
        }

        [System.Web.Http.HttpPost]
        public void Delete(int id)
        {
            Ingredient ingredient = db.Ingredient.Find(id);
            db.Ingredient.Remove(ingredient);
            db.SaveChanges();
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