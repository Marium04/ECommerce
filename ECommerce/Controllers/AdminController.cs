using ECommerce.Filters;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult ListProducts()
        {
            var db = new ShoppingEntities();
            var q = (from product in db.Products select product);
            return View(q);
        }

        // GET:
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult AddProduct(FormCollection c)
        {
            var name = c["Name"];
            var description = c["Description"];
            var price = Convert.ToDouble(c["Price"]);
            var category = c["Category"];
            var code = c["Code"];
            WebImage photo = null;
            var newFileName = "";
            var imagePath = "";
            var imageThumbPath = "";
            if (ModelState.IsValid)
            {
                photo = WebImage.GetImageFromRequest();
                if (photo != null)
                {
                    newFileName = Path.GetFileName(photo.FileName);

                    imagePath = @"images/Products/" + newFileName;

                    photo.Save(@"~/" + imagePath);


                    imageThumbPath = @"images/Products/thumbnails/" + newFileName;
                    photo.Resize(width: 165, height: 35, preserveAspectRatio: false,
                       preventEnlarge: true);
                    photo.Save(@"~/" + imageThumbPath);
                }
                var db = new ShoppingEntities();
                var im = "/" + imagePath;
                var im1 = "/" + imageThumbPath;
                Product p = new Product();
                p.Name = name;
                p.Category = category;
                p.Price = price;
                p.Description = description;
                p.Code = (code);
                p.Img = im;
                p.Thumbnail = im1;
                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("ListProducts");
            }
            else
                return View();
        }

        // GET : DeleteProduct
        [Authorize]
        [InitializeSimpleMembership]
        public ActionResult DeleteProduct(int id)
        {
            var db= new ShoppingEntities();
            Product p = db.Products.Find(id);
            return View(p);
        }
        
        //POST : Delete
        [Authorize]
        [InitializeSimpleMembership]
        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = new ShoppingEntities();
            Product p = db.Products.Find(id);
            db.Products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }
        
        [InitializeSimpleMembership]
        public ActionResult UpdateProduct(int id)
        {
            var db = new ShoppingEntities();
            Product pro = db.Products.Find(id);
            return View(pro);
        }

        //
        // POST: 
        [HttpPost]
        [InitializeSimpleMembership]
        public ActionResult UpdateProduct(Product p)
        {
            WebImage photo = null;
            var newFileName = "";
            var imagePath = "";
            var imageThumbPath = "";
            photo = WebImage.GetImageFromRequest();
            if (photo != null)
            {
                newFileName = Path.GetFileName(photo.FileName);
                imagePath = @"images/Products/" + newFileName;
                photo.Save(@"~/" + imagePath);
                imageThumbPath = @"images/Products/thumbnails/" + newFileName;
                photo.Resize(width: 165, height: 35, preserveAspectRatio: false,preventEnlarge: true);
                photo.Save(@"~/" + imageThumbPath);
            }
            var db = new ShoppingEntities();
            var im = "/" + imagePath;
            var im1 = "/" + imageThumbPath;
            p.Img = im;
            p.Thumbnail = im1;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListProducts");
            
        }
    }
}