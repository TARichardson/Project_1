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
    public class UserReposController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();
        private UserRepo ur = new UserRepo();


        // GET: UserRepo
        public ActionResult Index()
        {
            //int? id = Int32.Parse(Session["UserID"].ToString());

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //User user = db.Users.Find(id);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}

            //string query = "select * from Customers where UserID=2";

            //var cus = db.Customers.SqlQuery(query).FirstOrDefault<Customer>();
            //ur.CurrentUser = user;
            //ur.CurrentCustomer = cus;
            //return View(ur);
            return View();
        }
    }
}