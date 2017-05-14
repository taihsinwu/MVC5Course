using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [SharedViewBag]
        [HandleError]
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";
            throw new ArgumentException("Error Handle"); //自訂例外錯誤

            return View();
        }
        [SharedViewBag]
        public ActionResult PartialAbout()
        {
            //ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest()) //判斷是否有Ajax回應，只有MVC才能用
            {
                return PartialView("About"); //回傳沒有html的view
                //在http://localhost/Home/PartialAbout中，按F12，切換Console，打上 $.get('/Home/PartialAbout',function(data){alert(data);}); (←此為ajax)驗證
            }
            else
            {
                return View("About");
            }    
        }

        public ActionResult Unkonw()
        {
            return View();
        }

        //轉址到Home/Index
        public ActionResult SomeAction()
        {
            return PartialView("SuccessRedirect", "/");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult GetFile()
        {
            return File(Server.MapPath("~/Content/highlight.jpg"),"image/png","HighLight.png");
        }

        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json(db.Product.Take(5),
                        JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewTest()
        {
            ViewBag.IsEnable = true;
            return View();
        }

        public ActionResult RazorTest()
        {
            int[] data = new int[]{1,2,3,4,5};

            return PartialView(data);
        }
    }
}