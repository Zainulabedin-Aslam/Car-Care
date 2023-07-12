using CarServiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CarServiceManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        BookAMechanicEntities db = new BookAMechanicEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        //Managing customer's profile
        public ActionResult ManageCustomer()
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
            try
            {
                bool isAdded = (bool)TempData["isAdded"];
                string feedback = (string)TempData["feedback"];
                ViewBag.isAdded = isAdded;
                ViewBag.feedback = feedback;
            }
            catch (Exception)
            {
            }
            tbl_customer user = Session["User"] as tbl_customer;
            return View(user);
        }

        //booking a mechanic
        public ActionResult BookAMechanic()
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

            using (var db = new BookAMechanicEntities())
            {
                List<tbl_mechanic> AllEmployees = db.tbl_mechanic.ToList();
                int count = db.tbl_mechanic.Count();
                ViewBag.count = count;
                return View(AllEmployees);
            }

        }

        //previewing previous order's
        public ActionResult OrderHistory()
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
            List<tbl_order> orders = db.tbl_order.Where(x => x.customer_id == user.id).ToList();
            return View(orders.Where(x => x.status == "completed" || x.status == "cancelled"));
        }

        //previewing current order
        public ActionResult CurrentOrder()
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
            var user = Session["User"] as tbl_customer;
            List<tbl_order> currOrders = db.tbl_order.Where(x => x.customer_id == user.id).ToList();
            return View(currOrders.Where(x => x.status == "pending" || x.status == "ongoing").ToList());
        }


        //Action methods that perform CRUD functionality
        [HttpPost]
        public ActionResult EditCustomer(tbl_customer emp) {
            tbl_customer existingEmployee = db.tbl_customer.Find(emp.id);
            bool isAdded = false;
            try
            {
                existingEmployee.firstname = emp.firstname;
                existingEmployee.lastname = emp.lastname;
                existingEmployee.username = emp.username;
                existingEmployee.password = emp.password;
                existingEmployee.contact = emp.contact;
                existingEmployee.address = emp.address;
                existingEmployee.isBooked = emp.isBooked;
                existingEmployee.isOnline = emp.isOnline;
                existingEmployee.avg_rating = emp.avg_rating;
                db.SaveChanges();
                TempData["feedback"] = "Record Updated Successfully";
                isAdded = true;
            }
            catch (Exception ex)
            {
                TempData["feedback"] = "Unable to Update Record";
            }
            TempData["isAdded"] = isAdded;
            Session["User"] = existingEmployee;
            return RedirectToAction("ManageCustomer");
        }

        public ActionResult BookNow(int id) {
            if (Service == null)
            {
                Console.WriteLine("Service not selected");
            }
            else
            {
                var user = db.tbl_mechanic.Where(x => x.id == id).FirstOrDefault();
                return View(user);
            }
        } }

        [HttpPost]
        public ActionResult BookNow(int id,string[] service,string serviceCost) {
            var user = Session["User"] as tbl_customer;
            string services = "";
            for (int i = 0; i < service.Length; i++)
            {
                if (i+1 == service.Length)
                {
                    services += service[i];                    
                }
                else
                {
                    services += service[i] + ",";
                }
            }
            tbl_order order = new tbl_order
            {
                customer_id = user.id,
                mechanic_id = id,
                order_date = DateTime.Now,
                order_price = Convert.ToInt32(serviceCost),
                service = services,
                status = "pending"
            };
            bool isBooked = false;
            try
            {
                db.tbl_order.Add(order);
                db.SaveChanges();
                isBooked = true;
                TempData["Feedback"] = "Order Requested Successfully!";
            }
            catch (Exception ex)
            {
                isBooked = false;
                TempData["Feedback"] = "Failed To Request Order! Please Try again";
            }
            return RedirectToAction("CurrentOrder","Customer");
        }


        public ActionResult CompleteOrder(int id) {
            bool isDeclined = false;
            tbl_order order = db.tbl_order.Where(x => x.id == id).FirstOrDefault();
            try
            {                
                order.status = "review";
                db.SaveChanges();                
            }
            catch (Exception ex)
            {
                
            }
            Session["orderId"] = id;
            return RedirectToAction("ReviewOrder");
        }

        public ActionResult ReviewOrder() {
            int orderid = (int)Session["orderId"];
            return View(db.tbl_order.Where(x=>x.id == orderid).FirstOrDefault());
        }

        public ActionResult ViewDetails(int id) {
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

        [HttpPost]
        public ActionResult ReviewOrder(int id,string review, string rating) {
            int orderid = (int)Session["orderId"];
            tbl_order order = db.tbl_order.Where(x => x.id == id).FirstOrDefault();
            try
            {
                tbl_completed complete = new tbl_completed
                {
                    order_id = order.id,
                    customer_review = review,
                    client_id = order.tbl_customer.id,
                    client_rating = Convert.ToInt32(rating),
                    mechanic_id = order.tbl_mechanic.id,
                    complete_date = DateTime.Now,
                    mechanic_review = "Yet to be reviewed"
                };                
                order.tbl_completed.Add(complete);
                db.SaveChanges();     
            }
            catch (Exception ex)
            {
                
            }
            return RedirectToAction("OrderHistory");
        }

        public ActionResult CancelOrder(int id) {
            bool isDeclined = false;
            tbl_order order = db.tbl_order.Where(x => x.id == id).FirstOrDefault();
            try
            {
                tbl_cancel cancel = new tbl_cancel
                {
                    order_id = order.id,
                    cancelled_by = "Customer",
                    reason = "No Reason"
                };
                order.status = "cancelled";
                order.tbl_customer.isBooked = false;
                order.tbl_mechanic.isBooked = false;
                order.tbl_cancel.Add(cancel);
                db.SaveChanges();
                isDeclined = true;
            }
            catch (Exception ex)
            {
                
            }
            return RedirectToAction("OrderHistory");
        }
    }
}