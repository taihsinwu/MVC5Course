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
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
    public abstract class BaseController : Controller //設為abstract抽象類別
    {
        protected FabricsEntities db = new FabricsEntities();
        [LocalOnly]
        public ActionResult Debug()
        {
            return Content("hi");
        }

        ///// <summary>
        ///// 進入找不到的網址時，強制回到Home/Index
        ///// </summary>
        ///// <param name="actionName"></param>
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    this.RedirectToAction("Index", "Home").ExecuteResult(this.ControllerContext);
        //}
    }
}