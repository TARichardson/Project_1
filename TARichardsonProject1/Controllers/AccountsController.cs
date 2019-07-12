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
    public class AccountsController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();
        private TransactionType ttype;
        private AccountType atype;
        private TransferRepo tf = new TransferRepo();


        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Customer).Include(a => a.AccountType);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
            ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName");
            return View();
        }
        public ActionResult Withdraw(int? id)
        {
            try
            {
                string uid = Session["UserID"].ToString();
                User user = db.Users.Find(Int32.Parse(uid));
                var cus = db.Customers.Where(cusq => cusq.UserID == user.UserID).FirstOrDefault();
                Account account = db.Accounts.Find(id);
                if (account == null || user == null)
                {
                    return HttpNotFound();
                }
                tf.CurrentUser = user;
                tf.CurrentCustomer = cus;
                tf.FromAccount = account;

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
                ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName");
                return View(tf);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Withdraw(TransferRepo trepo)
        {
            decimal? sum = trepo.ToAccount.Balances;
            
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName");
            ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName");
            return View();
        }
        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,TypeID,InterestRate,StartingBalance,EndingBalance,AccountOpenDate,BeginningStatment,NextStatment,Balances,CustomerID")] Account account)
        {
            try
            {
                string id = Session["UserID"].ToString();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(Int32.Parse(id));

                if (ModelState.IsValid && user != null)
                {
                    var cus = db.Customers.Where(cusq => cusq.UserID == user.UserID).FirstOrDefault();
                    if (cus != null)
                    {
                        ttype = db.TransactionTypes.Where(ty => ty.TransactionName == "OPN").FirstOrDefault();
                        atype = db.AccountTypes.Where(aty => aty.TypeID == account.TypeID).FirstOrDefault();

                        account.CustomerID = cus.CustomerID;
                        account.Balances = account.StartingBalance;
                        account.BeginningStatment = DateTime.UtcNow;
                        account.NextStatment = DateTime.UtcNow.AddDays(30);
                        account.AccountOpenDate = account.BeginningStatment;
                        account.InterestRate = 10;
                        //Transaction transaction = transaction.TransTypeID
                        account.Transactions.Add(new Transaction()
                        {
                            TransTypeID = ttype.TransactionID,
                            Date = DateTime.UtcNow,
                            Log = $"{account.AccountOpenDate} Account Open with balances of {account.Balances}"

                        });

                        // term Account
                        if(account.TypeID == 2)
                        {
                            account.TermAccounts.Add(new TermAccount()
                            {
                                DateOfMaturity = DateTime.UtcNow.AddDays(30)
                            });
                        }

                        // Loan Account
                        if (account.TypeID == 4)
                        {
                            account.LoanAccounts.Add(new LoanAccount()
                            {
                                LoanAmount = (decimal)account.StartingBalance,
                                LoanTransfered = false
                            });
                        }

                        db.Accounts.Add(account);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", account.CustomerID);
                ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName", account.TypeID);
                return View(account);
            }
            catch (Exception)
            {
                return RedirectToAction("Create");

            }
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", account.CustomerID);
            ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName", account.TypeID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,TypeID,InterestRate,StartingBalance,EndingBalance,AccountOpenDate,BeginningStatment,NextStatment,Balances,CustomerID")] Account account, Account account2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "FirstName", account.CustomerID);
            ViewBag.TypeID = new SelectList(db.AccountTypes, "TypeID", "TypeName", account.TypeID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
    }
}
