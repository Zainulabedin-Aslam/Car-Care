using CarServiceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarServiceManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        BookAMechanicEntities db = new BookAMechanicEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            if (Session["Error"] != null)
            {
                ViewBag.error = Session["Error"] as string;
                Session["Error"] = null;
            }
            if (Session["UserNotValid"] != null)
            {
                ViewBag.error = Session["UserNotValid"] as string;
                Session["UserNotValid"] = null;
            }

            if (Session["pass"] != null)
            {
                ViewBag.error = "Password Changed Successfully :)";
                Session["pass"] = null;
            }

            ViewBag.feedback = TempData["Feedback"] as String;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password, string role)
        {
            if (role == "admin")
            {
                using (var db = new BookAMechanicEntities())
                {
                    bool isValid = false;

                    var _Employee = db.tbl_admin.FirstOrDefault(x => x.username == username);
                    if (_Employee == null)
                    {
                        //user not found 
                        Session["UserNotValid"] = " Username does not exsist";
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        isValid = _Employee.password == password;
                    }

                    if (!isValid)
                    {
                        Session["UserNotValid"] = " Invalid Username or Password.";
                        return RedirectToAction("Login", "Login");
                    }

                    Session["User"] = _Employee;
                    Session["isCust"] = false;
                    Session["isAdmin"] = true;
                    Session["isMech"] = false;


                    if (isValid)
                    {
                        Session["UserName"] = _Employee.username;
                        FormsAuthentication.SetAuthCookie(username, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else if (role =="customer")
            {
                using (var db = new BookAMechanicEntities())
                {
                    bool isValid = false;

                    var _Employee = db.tbl_customer.FirstOrDefault(x => x.username == username);
                    if (_Employee == null)
                    {
                        //user not found 
                        Session["UserNotValid"] = " Username does not exsist";
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        isValid = _Employee.password == password;
                    }

                    if (!isValid)
                    {
                        Session["UserNotValid"] = " Invalid Username or Password.";
                        return RedirectToAction("Login", "Login");
                    }

                    Session["User"] = _Employee;
                    Session["isCust"] = true;
                    Session["isAdmin"] = false;
                    Session["isMech"] = false;

                    if (isValid)
                    {
                        Session["UserName"] = _Employee.username;
                        FormsAuthentication.SetAuthCookie(username, false);
                        return RedirectToAction("CustomerIndex", "Home");
                    }
                }
            }
            else if (role == "mechanic")
            {
                using (var db = new BookAMechanicEntities())
                {
                    bool isValid = false;

                    var _Employee = db.tbl_mechanic.FirstOrDefault(x => x.username == username);
                    if (_Employee == null)
                    {
                        //user not found 
                        Session["UserNotValid"] = " Username does not exsist";
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        isValid = _Employee.password == password;
                    }

                    if (!isValid)
                    {
                        Session["UserNotValid"] = " Invalid Username or Password.";
                        return RedirectToAction("Login", "Login");
                    }

                    Session["User"] = _Employee;
                    Session["isCust"] = false;
                    Session["isAdmin"] = false;
                    Session["isMech"] = true;


                    if (isValid)
                    {
                        Session["UserName"] = _Employee.username;
                        FormsAuthentication.SetAuthCookie(username, false);
                        return RedirectToAction("MechanicIndex", "Home");
                    }
                }
            }
            
            return View();
        
        }
        
        public ActionResult SignUp()
        {
            return View();
        } 
        public ActionResult SignUpCustomer()
        {
            return View();
        } 
        public ActionResult SignUpMechanic()
        {
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["User"] != null)
            {
                Session.Clear();
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult SignUpCustomer(tbl_customer user)
        {
            Boolean isSignedUp = false;
            if (ModelState.IsValid)
            {
                tbl_customer User = new tbl_customer
                {
                    firstname = user.firstname,
                    lastname = user.lastname,
                    address = user.address,
                    contact = user.contact,
                    username = user.username,
                    password = user.password,
                    avg_rating = 0,
                    isOnline = false,
                    isBooked = false
                };
                try
                {
                    db.tbl_customer.Add(User);
                    db.SaveChanges();
                    isSignedUp = true;
                    TempData["Feedback"] = "User Signed Up Successfully!";
                }
                catch (Exception ex)
                {
                    isSignedUp = false;
                    TempData["Feedback"] = "Failed To Sign you Up! Please Try again";
                }
            }
            ModelState.Clear();
            if (isSignedUp)
            {
                return RedirectToAction("Login");
            }
            ViewBag.feedback = TempData["Feedback"] as String;
            ViewBag.signedUp = isSignedUp;
            return View();
        }        
        [HttpPost]
        public ActionResult SignUpMechanic(tbl_mechanic user)
        {
            Boolean isSignedUp = false;
            if (ModelState.IsValid)
            {
                tbl_mechanic User = new tbl_mechanic
                {
                    firstname = user.firstname,
                    lastname = user.lastname,
                    garageName = user.garageName,
                    address = user.address,
                    contact = user.contact,
                    username = user.username,
                    password = user.password,
                    avg_rating = 0,
                    isOnline = false,
                    isBooked = false
                };
                try
                {
                    db.tbl_mechanic.Add(User);
                    db.SaveChanges();
                    isSignedUp = true;
                    TempData["Feedback"] = "User Signed Up Successfully!";
                }
                catch (Exception ex)
                {
                    isSignedUp = false;
                    TempData["Feedback"] = "Failed To Sign you Up! Please Try again";
                }
            }
            ModelState.Clear();
            if (isSignedUp)
            {
                return RedirectToAction("Login");
            }
            ViewBag.feedback = TempData["Feedback"] as String;
            ViewBag.signedUp = isSignedUp;
            return View();
        }

    }
}