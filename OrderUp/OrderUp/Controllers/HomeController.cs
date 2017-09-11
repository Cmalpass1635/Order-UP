using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrderUp.Models;
using Microsoft.AspNet.Identity;

namespace OrderUp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Menu()
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from m in db.MENUs select m;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult Discount()
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from d in db.DISCOUNTs select d;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult Tax()
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.TAXs select t;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult Customerorder()
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.CUSTOMERORDERs select t;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult CreateOrder()
        {
            string userID = User.Identity.GetUserName();
            CUSTOMERORDER co = new CUSTOMERORDER(); 
            co.ServerName=userID;
            co.DiscountName = "No Discount";
            var db = new OrderUp.Models.ApplicationDbContext();

            db.CUSTOMERORDERs.Add(co);
            db.SaveChanges();
            return RedirectToAction("EditOrder", new { orderid = co.OrderID });
        }

        [Authorize]
        public ActionResult Orderitem()
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.ORDERITEMs select t;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult EditOrder(int orderid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.ORDERITEMs where t.OrderID == orderid select t;
            var qry2 = from o in db.CUSTOMERORDERs where o.OrderID == orderid select o;
            var qry3 = from d in db.DISCOUNTs select d.DiscountName;
            SelectList dlist = new SelectList(qry3.ToList());
            ViewBag.Discount = dlist;
            ViewBag.OrderItems = qry.ToList();
            return View(qry2.ToList()[0]);
        }

        [Authorize]
        public ActionResult AddItem(int orderid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from m in db.MENUs select m;
            ViewBag.orderid = orderid;
            return View(qry.ToList());
        }

        [Authorize]
        public ActionResult SelectItem(int orderid, String foodname, decimal foodprice)
        {
            ORDERITEM oi = new ORDERITEM();
            CUSTOMERORDER co = new CUSTOMERORDER();
            ApplicationDbContext db = new ApplicationDbContext();

            var qry = from c in db.CUSTOMERORDERs where c.OrderID == orderid select c;
            co = qry.ToList()[0];

            co.Subtotal += foodprice;

            this.Compute(ref co);

            oi.OrderID = orderid;
            oi.FoodName = foodname;
            oi.FoodPrice = foodprice;
            
            db.ORDERITEMs.Add(oi);
            db.SaveChanges();
            return RedirectToAction("EditOrder", new { orderid = oi.OrderID });
        }

        [Authorize]
        public ActionResult SubmitOrder(int orderid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.CUSTOMERORDERs where t.OrderID == orderid select t;
            qry.ToList()[0].OrderDate = DateTime.Now.ToString();
            db.SaveChanges();
            return RedirectToAction("Customerorder");
        }

        [Authorize]
        public ActionResult DeleteOrder(int orderid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from t in db.CUSTOMERORDERs where t.OrderID == orderid select t;
            db.CUSTOMERORDERs.Remove(qry.ToList()[0]);
            db.SaveChanges();
            return RedirectToAction("Customerorder");
        }

        [Authorize]
        public ActionResult DeleteItem(int detailid)
        {
            CUSTOMERORDER co = new CUSTOMERORDER();
            var db = new OrderUp.Models.ApplicationDbContext();

            var qry = from d in db.ORDERITEMs where d.DetailID == detailid select d;
            int orderid = (int) qry.ToList()[0].OrderID;
            decimal foodprice = (decimal)qry.ToList()[0].FoodPrice;

            var qry2 = from c in db.CUSTOMERORDERs where c.OrderID == orderid select c;
            co = qry2.ToList()[0];

            co.Subtotal -= foodprice;

            this.Compute(ref co);

            db.ORDERITEMs.Remove(qry.ToList()[0]);
            db.SaveChanges();
            return RedirectToAction("EditOrder", new { orderid = orderid });
        }

        public void Compute(ref CUSTOMERORDER co)
        {
            DISCOUNT dc = new DISCOUNT();
            int tax;
            ApplicationDbContext db = new ApplicationDbContext();

            string discountname = co.DiscountName;
            var qry = from d in db.DISCOUNTs where d.DiscountName == discountname select d;
            var qry2 = from t in db.TAXs select t.TaxAmount;

            dc = qry.ToList()[0];
            tax = qry2.ToList().Sum();

            if (dc.DiscountType.ToLower() == "fixed")
            {
                co.DiscountAmount = dc.DiscountAmount;
                co.Pretax = co.Subtotal - co.DiscountAmount;
                if (co.Pretax < 0)
                {
                    co.Pretax = 0;
                }
            }
            else if (dc.DiscountType.ToLower() == "percent")
            {
                decimal percent = dc.DiscountAmount / 100;
                co.DiscountAmount = co.Subtotal * percent;
                co.Pretax = co.Subtotal - co.DiscountAmount;
            }

            decimal taxpercent = (decimal)tax / 100;
            co.Tax = co.Pretax * taxpercent;
            co.Total = co.Pretax + co.Tax;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(string discountname, int orderid)
        {
             var db = new OrderUp.Models.ApplicationDbContext();
            CUSTOMERORDER co = new CUSTOMERORDER();
            var qry = from c in db.CUSTOMERORDERs where c.OrderID == orderid select c;
            co = qry.ToList()[0];
            co.DiscountName = discountname;
            this.Compute(ref co);
            db.SaveChanges();
            return RedirectToAction("EditOrder", new { orderid = orderid });
           
        }


        [Authorize]
        public ActionResult SaveDiscount(int orderid, string discountname)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            CUSTOMERORDER co = new CUSTOMERORDER();
            var qry = from c in db.CUSTOMERORDERs where c.OrderID == orderid select c;
            co = qry.ToList()[0];
            co.DiscountName = discountname;
            db.SaveChanges();
            return RedirectToAction("EditOrder", new { orderid = orderid });
        }

        [Authorize]
        public ActionResult CreateMenu()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenu(string FoodName, decimal Price, string Category)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            MENU mn = new MENU();
            mn.FoodName = FoodName;
            mn.Price = Price;
            mn.Category = Category;
            db.MENUs.Add(mn);
            db.SaveChanges();
            return RedirectToAction("Menu");
        }

        [Authorize]
        public ActionResult EditMenu(int menuid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from m in db.MENUs where m.MenuID == menuid select m;
            MENU mn = new MENU();
            mn = qry.ToList()[0];
            return View(mn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu(string FoodName, decimal Price, string Category)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from m in db.MENUs where m.FoodName == FoodName select m;
            MENU mn = new MENU();
            mn = qry.ToList()[0];
            mn.FoodName = FoodName;
            mn.Price = Price;
            mn.Category = Category;
            db.SaveChanges();
            return RedirectToAction("Menu");
        }

        [Authorize]
        public ActionResult DeleteMenu(int menuid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            var qry = from m in db.MENUs where m.MenuID == menuid select m;
            MENU mn = new MENU();
            mn = qry.ToList()[0];
            db.MENUs.Remove(mn);
            db.SaveChanges();
            return RedirectToAction("Menu");
        }

        [Authorize]
        public ActionResult CreateDiscount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDiscount(string DiscountName, string DiscountType, decimal DiscountAmount)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            DISCOUNT dc = new DISCOUNT();
            dc.DiscountName = DiscountName;
            dc.DiscountType = DiscountType;
            dc.DiscountAmount = DiscountAmount;
            db.DISCOUNTs.Add(dc);
            db.SaveChanges();
            return RedirectToAction("Discount");
        }

        [Authorize]
        public ActionResult EditDiscount(int discountid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            DISCOUNT dc = new DISCOUNT();
            var qry = from d in db.DISCOUNTs where d.DiscountID == discountid select d;
            dc = qry.ToList()[0];
            return View(dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDiscount(string DiscountName, string DiscountType, decimal DiscountAmount)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            DISCOUNT dc = new DISCOUNT();
            var qry = from d in db.DISCOUNTs where d.DiscountName == DiscountName select d;
            dc = qry.ToList()[0];
            dc.DiscountName = DiscountName;
            dc.DiscountType = DiscountType;
            dc.DiscountAmount = DiscountAmount;
            db.SaveChanges();
            return RedirectToAction("Discount");
        }

        [Authorize]
        public ActionResult DeleteDiscount(int discountid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            DISCOUNT dc = new DISCOUNT();
            var qry = from d in db.DISCOUNTs where d.DiscountID == discountid select d;
            dc = qry.ToList()[0];
            db.DISCOUNTs.Remove(dc);
            db.SaveChanges();
            return RedirectToAction("Discount");
        }

        [Authorize]
        public ActionResult CreateTax()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTax(string TaxName, int TaxAmount)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            TAX tx = new TAX();
            tx.TaxName = TaxName;
            tx.TaxAmount = TaxAmount;
            db.TAXs.Add(tx);
            db.SaveChanges();
            return RedirectToAction("Tax");
        }

        [Authorize]
        public ActionResult EditTax(int taxid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            TAX tx = new TAX();
            var qry = from t in db.TAXs where t.TaxID == taxid select t;
            tx = qry.ToList()[0];
            return View(tx);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTax(string TaxName, int TaxAmount)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            TAX tx = new TAX();
            var qry = from t in db.TAXs where t.TaxName == TaxName select t;
            tx = qry.ToList()[0];
            tx.TaxName = TaxName;
            tx.TaxAmount = TaxAmount;
            db.SaveChanges();
            return RedirectToAction("Tax");
        }

        [Authorize]
        public ActionResult DeleteTax(int taxid)
        {
            var db = new OrderUp.Models.ApplicationDbContext();
            TAX tx = new TAX();
            var qry = from t in db.TAXs where t.TaxID == taxid select t;
            tx = qry.ToList()[0];
            db.TAXs.Remove(tx);
            db.SaveChanges();
            return RedirectToAction("Tax");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}