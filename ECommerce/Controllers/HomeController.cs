using ECommerce.Filters;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to my ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "App description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Product()
        {
            ViewBag.Message = "Products page.";
            var db = new ShoppingEntities();
            var q = from s in db.Products select s;
            return View(q);
        }

        public ActionResult ProductDetails(int id)
        {
            var db = new ShoppingEntities();
            Product pro= db.Products.Find(id);
            return View(pro);
        }

        [HttpPost,ActionName("ProductDetails")]
        [InitializeSimpleMembership]
        public ActionResult ProductDetail(int id)
        {
            var db = new ShoppingEntities();
            Product p = db.Products.Find(id);
            if (!WebSecurity.IsAuthenticated)
            {
                Response.Cookies["error"].Value = "You must login before adding items to cart.";
                return View(p);
            }
            return RedirectToAction("AddToCart/"+p.Code);
        }

        public ActionResult AddToCart(string id)
        {
            Response.Cookies["error"].Value = "";
            if (Request.Cookies["items"] != null)
            {
                int items = (Convert.ToInt32(Request.Cookies["items"].Value));
                items++;
                Response.Cookies["items"].Value = items.ToString();
            }
            if (Request.Cookies["items"] == null)
            {
                Response.Cookies["items"].Value = "1";
            }
            if (Request.Cookies[id] == null)
            {
                Response.Cookies[id].Value = "1";
            }
            else
            {
                int i = Convert.ToInt32(Request.Cookies[id].Value);
                i++;
                Response.Cookies[id].Value = i.ToString();
            }
            return RedirectToAction("Product");
        }
        public ActionResult ViewCart()
        {
            var items = 0;
            if (Request.Cookies["items"] != null)
            {
                items = Convert.ToInt32(Request.Cookies["items"].Value);
            }
            double bill = 0.0d;
            var db = new ShoppingEntities();
            var q = from t in db.Products select t;
            return View(q);
        }
    }
}
