using CarServiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarServiceManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        BookAMechanicEntities db = new BookAMechanicEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageCustomer()
        {
            try
            {
                bool isAdmin = (bool)Session["isAdmin"];
                if (!isAdmin)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");

            }
            using (var db = new BookAMechanicEntities())
            {
                List<tbl_customer> AllEmployees = db.tbl_customer.ToList();
                int count = db.tbl_customer.Count();
                ViewBag.count = count;
                return View(AllEmployees);
            }
        }

        public ActionResult ManageMechanic()
        {
            try
            {
                bool isAdmin = (bool)Session["isAdmin"];
                if (!isAdmin)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");

            }
            using (var db = new BookAMechanicEntities())
            {
                List<tbl_mechanic> AllEmployees = db.tbl_mechanic.ToList();
                int count = db.tbl_mechanic.Count();
                ViewBag.count = count;
                return View(AllEmployees);
            }
        }

        public ActionResult AllOrders()
        {
            try
            {
                bool isAdmin = (bool)Session["isAdmin"];
                if (!isAdmin)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");

            }
            return View(db.tbl_order.Where(x => x.status == "completed" || x.status == "cancelled"));
        }

        public ActionResult ViewDetails(int id)
        {
            tbl_admin admin = Session["User"] as tbl_admin;
            ViewBag.username = admin.username;
            tbl_order order = db.tbl_order.Where(x => x.id == id).FirstOrDefault();
            if (order.status.Equals("completed"))
            {
                ViewBag.completed = db.tbl_completed.Where(x => x.order_id == id).FirstOrDefault();
            }
            else
            {
                ViewBag.cancelled = db.tbl_cancel.Where(x => x.order_id == id).FirstOrDefault();
            }
            return View(db.tbl_order.Where(x => x.id == id).FirstOrDefault());
        }


        public ActionResult CurrOrders()
        {
            try
            {
                bool isAdmin = (bool)Session["isAdmin"];
                if (!isAdmin)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");

            }
            return View();
        }


        ///Delete methods for customers and mechanics
        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            int count = db.tbl_customer.Where(x => x.id == id).Count();
            if (count > 0)
            {
                tbl_customer deletedEmp = db.tbl_customer.Where(x => x.id == id).FirstOrDefault();
                db.tbl_customer.Remove(deletedEmp);
                db.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            return Json("User does not exist", JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult DeleteMechanic(int id)
        {
            int count = db.tbl_mechanic.Where(x => x.id == id).Count();
            if (count > 0)
            {
                tbl_mechanic deletedEmp = db.tbl_mechanic.Where(x => x.id == id).FirstOrDefault();
                db.tbl_mechanic.Remove(deletedEmp);
                db.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            return Json("User does not exist", JsonRequestBehavior.DenyGet);
        }
    }
}