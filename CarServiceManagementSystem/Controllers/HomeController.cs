using CarServiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarServiceManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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
            tbl_admin user = Session["User"] as tbl_admin;
            ViewBag.user = user;
            return View();
        } 
        public ActionResult CustomerIndex()
        {
            try
            {

                bool isCust = (bool)Session["isCust"];
                if (!isCust)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");

            }
            tbl_customer user = Session["User"] as tbl_customer;
            ViewBag.user = user;
            return View();
        } 
        public ActionResult MechanicIndex()
        {
            try
            {
                bool isMech = (bool)Session["isMech"];
                if (!isMech)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_mechanic user = Session["User"] as tbl_mechanic;
            ViewBag.user = user;
            return View();
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