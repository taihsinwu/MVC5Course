﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class ProductsController : BaseController
    {
        ProductRepository repo = RepositoryHelper.GetProductRepository();
        //private FabricsEntities db = new FabricsEntities();

        // GET: Products
        public ActionResult Index(bool Active = true)
        {
            var data = repo.GetAllData(Active, showAll: false).OrderByDescending(p => p.ProductId).Take(10);

            //var data = db.Product
            //    .Where(p => p.Active.HasValue && p.Active.Value == Active)
            //    .OrderByDescending(p => p.ProductId).Take(10);

            return View(data);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.GetOneDataById(id.Value);
            //Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            //顯示表單
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType=typeof(DbUpdateException),View ="Error_DBUpdateException")]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            //接收表單傳來的資料
            //if (ModelState.IsValid)
            {
                repo.Add(product);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.GetOneDataById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,FormCollection form)
        {
            //原本帶入的參數：[Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)

            //if (ModelState.IsValid)
            //{
            //    //db.Entry(product).State = EntityState.Modified;
            //    repo.Update(product);
            //    repo.UnitOfWork.Commit();
            //    return RedirectToAction("Index");
            //}

            var product = repo.GetOneDataById(id);
            if (TryUpdateModel(product, new string[] {"ProductID","ProductName","Price","Ative","Stock"}))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.GetOneDataById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = repo.GetOneDataById(id);

            repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false; //不須驗證

            repo.Delete(product);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ListProducts(ProductList_SearchVM search)
        {
            var data = repo.GetAllData(true);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(search.queryName))
                {
                    data = data.Where(p => p.ProductName.Contains(search.queryName));
                }
               
                //data = data.Where(p => p.Price >= search.queryPrice);
                data = data.Where(p => p.Stock > search.queryStockFrom && p.Stock < search.queryStockTo);
            }

            ViewData.Model = data
                                .Select(p => new ProductLiteVM()
                                {
                                    ProductID = p.ProductId,
                                    ProductName = p.ProductName,
                                    Price = p.Price,
                                    Stock = p.Stock
                                }).Take(10)
                                 .OrderByDescending(p => p.ProductID);
            return View();
        }

        public ActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(ProductLiteVM data)
        {
            if (ModelState.IsValid)
            {
                //儲存資料進資料庫
                TempData["CreateProduct_Result"] = "新增成功嚕";
                return RedirectToAction("ListProducts");
            }
            //驗證失敗，繼續顯示原本的表單
            return View();
        }
    }
}
