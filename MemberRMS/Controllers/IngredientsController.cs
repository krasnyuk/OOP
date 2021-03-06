﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MemberRMS.Models;
using Microsoft.Ajax.Utilities;
using WebGrease;

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
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Ingredient ingredient = db.Ingredient.Find(id);
            db.Ingredient.Remove(ingredient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public JsonResult IngredientCountJsonResult()
        {
            
          
            var result = (from p in db.Category.OrderBy(x => x.CategoryID)
                          select new CategoriesViewModel
                          {
                              CategoryID = p.CategoryID,
                              ParentCategoryID = p.ParentCategoryID,
                              Title = p.Title
                          }).ToList();
            var list = result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CategoriesData()
        {
            var result = (from p in db.Category.OrderBy(x => x.CategoryID)
                          select new CategoriesViewModel
                          {
                              CategoryID = p.CategoryID,
                              ParentCategoryID = p.ParentCategoryID,
                              Title = p.Title
                          }).ToList();
            var list = result;
            

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IngredientsData()
        {
            var result = (from p in db.Ingredient.OrderBy(x => x.IngredientID)
                          select new IngredientsViewModel()
                          {
                            IngredientID = p.IngredientID,
                            Title = p.Title
                          }).ToList();
            var list = result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private int _parentId;
        public JsonResult SubIngredientsData(int id)
        {
            _parentId = id;
            var result = (from p in db.ProductIngredient
                          where p.ProductID == _parentId
                          select new IngredientsViewModel()
                          {
                              IngredientID = (int) p.IngredientID,
                              CategoryID = p.IngredientID

                          }).ToList();
            var list = result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListSubIngredientsData()
        {
           
            var result = (from p in db.ProductIngredient
                          select new IngredientsViewModel()
                          {
                              IngredientID = (int)p.IngredientID,
                              //CategoryID = p.CategoryID,
                              //ParentCategoryID = p.ParentCategoryID,
                              //Title = p.Title
                          }).ToList();
            var list = result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInfoAboutIngredient(int id)
        {
           // _parentId = id;
            var result = (from p in db.Ingredient
                          where p.IngredientID == id
                          select new IngredientsViewModel()
                          {
                             // IngredientID = (int)p.IngredientID,
                             CategoryID = p.CategoryID,
                              //ParentCategoryID = p.ParentCategoryID,
                              //Title = p.Title
                          }).ToList();
            var list = result;


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryJsonResultData(string sidx, string sord, int page, int rows)
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
        public void Create( IngredientsViewModel ingredient)
        {
            if (ModelState.IsValid)
            {

                Ingredient item = new Ingredient
                {
                    CategoryID = Convert.ToInt16(ingredient.CategoryName),
                    IngredientID = (db.Ingredient.Max(x => x.IngredientID)+1),
                    Title = ingredient.Title,
                    LongDescription = ingredient.LongDescription,
                    Cost = ingredient.Cost,
                    Weight = ingredient.Weight
                };
               
               
                db.Ingredient.Add(item);
                db.SaveChanges();

                if (ingredient.SubIngredient != null)
                    foreach (Tmodel t in ingredient.SubIngredient)
                    {
                        ProductIngredient subIngredient = new ProductIngredient();
                        subIngredient.ProductID = item.IngredientID;
                        subIngredient.IngredientID = t.id;
                        subIngredient.ProductIngredientID = (db.ProductIngredient.Max(x => x.ProductIngredientID) + 1);
                        db.ProductIngredient.Add(subIngredient);
                        db.SaveChanges();
                    }
            }
            // действия по добавлению
        }

        [System.Web.Http.HttpPost]
        public void Edit( IngredientsViewModel ingredient) 
        {
            Ingredient item = new Ingredient
            {
                CategoryID = Convert.ToInt16(ingredient.CategoryName),
                IngredientID = ingredient.IngredientID,
                Title = ingredient.Title,
                LongDescription = ingredient.LongDescription,
                Cost = ingredient.Cost,
                Weight = ingredient.Weight
            };
            
            
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                if (ingredient.SubIngredient != null)
                    foreach (Tmodel t in ingredient.SubIngredient)
                    {
                        ProductIngredient subIngredient = new ProductIngredient();
                        subIngredient.ProductID = item.IngredientID;
                        subIngredient.IngredientID = t.id;
                        subIngredient.ProductIngredientID = (db.ProductIngredient.Max(x => x.ProductIngredientID) + 1);
                        db.ProductIngredient.Add(subIngredient);
                        db.SaveChanges();
                       
                    }
               




            }
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