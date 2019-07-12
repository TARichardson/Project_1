using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TARichardsonProject1.Models;

namespace TARichardsonProject1.Controllers
{
    public class HomeController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();
        private UserRepo ur = new UserRepo();
        private TransactionType ttype;
        private AccountType atype;

        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                string id = Session["UserID"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Overview");

            }
            catch (Exception)
            {
                return View();
            }
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login([Bind(Include = "UserName,Password")] User user)
        {
            //var users = db.Users.Include(u => u.Role);

            return View(user);
            //return RedirectToAction("Details", routeValues: "UsersController");
        }

        public ActionResult Overview()
        {
            try
            {
                string id = Session["UserID"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    User user = db.Users.Find(Int32.Parse(id));

                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    ur.CurrentUser = user;
                    try
                    {
                        // string query = "select * from Customers where UserID=2";

                        //var cus = db.Customers.SqlQuery(query).FirstOrDefault<Customer>();
                        var cus = db.Customers.Where(cusq => cusq.UserID == user.UserID).FirstOrDefault();
                        if (cus == null)
                        {
                            return HttpNotFound();
                        }

                        ur.CurrentCustomer = cus;
                        try
                        {
                            //string queryAc = "select * from Accounts where UserID=2";

                            var la = db.Accounts.Where(acc => acc.CustomerID == cus.CustomerID).Include((u => u.LoanAccounts)).Include(u => u.TermAccounts).ToList();

                            if (la == null)
                            {
                                return HttpNotFound();
                            }

                            ur.CurrentCustomer.Accounts = la;
                            return View(ur.CurrentCustomer.Accounts);


                        }
                        catch (Exception)
                        {
                            ur.CurrentCustomer = null;
                        }

                    }
                    catch (Exception)
                    {
                        ur.CurrentCustomer = null;
                    }

                }
                catch (Exception)
                {
                    ur.CurrentUser = null;
                }




                //string query = "select * from Customers where UserID=2";

                //var cus = db.Customers.SqlQuery(query).FirstOrDefault<Customer>();
                //ur.CurrentUser = user;
                //ur.CurrentCustomer = cus;
                //return View(ur);
                ViewBag.ID = id;
                return View(ur.CurrentCustomer.Accounts);
            }

            catch (Exception)
            {
                return View(ur.CurrentCustomer.Accounts);
            }
        }

    }
}