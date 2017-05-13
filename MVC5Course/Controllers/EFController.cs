using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();
        // GET: EF
        public ActionResult Index()
        {
            //.AsQueryable() => 最後執行時才查詢(延遲載入)
            //.AsEnumerable() => 直接查詢，再WHERE...等動作

            var allData = db.Product.AsQueryable();

            //.Take()、.ToList等，都會先查詢資料
            var data = allData.Where(p => p.Active == true && p.IsDeleted==false && 
                                     p.ProductName.Contains("Black")).Take(15)
                                     .OrderByDescending(p => p.ProductId);

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid == true)
            {
                db.Product.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid == true)
            {
                var delItem = db.Product.Find(id);

                ////取得Product的關聯資料表OrderLine，並將Orderline的資料刪除
                ////方法1
                //foreach(var item in delItem.OrderLine.ToList())
                //{
                //    db.OrderLine.Remove(item);
                //}
                ////方法2
                //db.OrderLine.RemoveRange(delItem.OrderLine);

                //db.Product.Remove(delItem);

                delItem.IsDeleted = true;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {         
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.Product.Find(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id,Product product)
        {
            if (ModelState.IsValid == true)
            {
                var item = db.Product.Find(id);
                item.ProductName = product.ProductName;
                item.Active = product.Active;
                item.Price = product.Price;
                item.Stock = product.Stock;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public void RemoveAll()
        {
            ////效能差
            //db.Product.RemoveRange(db.Product);
            //db.SaveChanges();

            //效能好
            db.Database.ExecuteSqlCommand("DELETE FROM dbo.Product");
        }

        public ActionResult Details(int id)
        {
            var data = db.Database.SqlQuery<Product>("SELECT * FROM dbo.Product WHERE ProductId=@p0", id).FirstOrDefault();

            return View(data);
        }
    }
}