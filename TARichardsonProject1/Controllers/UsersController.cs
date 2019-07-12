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
    public class UsersController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();
        private UserRepo ur = new UserRepo();

        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        //[ValidateAntiForgeryToken]
        //public ActionResult Login(UserName, Password)
        //{
        //    var users = db.Users.Include(u => u.Role);
        //    return View(users.ToList());
        //}

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            // string query = "select CustomerID from Customer where UserID=" + user.UserID.ToString();
            string query = "select * from Customers where UserID=2";

            var cus = db.Customers.SqlQuery(query).FirstOrDefault<Customer>();
            ViewBag.CustomerID = cus.CustomerID;
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }
        public ActionResult Login()
        {
            ViewBag.RoleID = "member";
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "UserID,UserName,Password,RoleID")] User user)
        {
            string userName = "TroyRic222";
            //if (userName == "" || password == "")
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //var user = db.Users.SqlQuery("select * from Users where UserName = @firstName AND Password = @password").FirstOrDefault<User>();
            string query = "select * from Users where UserName=\'" + userName + "\'";
            var user2 = db.Users.SqlQuery(query).FirstOrDefault<User>();

            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user2.RoleID);
            Session["UserID"] = user2.UserID.ToString();
            return RedirectToAction("Profiles");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Profiles()
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

                            var aa = db.Accounts.Where(acc => acc.CustomerID == cus.CustomerID).ToList();
                            if (aa == null)
                            {
                                return HttpNotFound();
                            }

                            ur.CurrentCustomer.Accounts = aa;


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
                return View(ur);
            }

            catch (Exception)
            {
                return View(ur);
            }
        }
    }
}
