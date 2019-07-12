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
    public class LoanAccountsController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();

        // GET: LoanAccounts
        public ActionResult Index()
        {
            var loanAccounts = db.LoanAccounts.Include(l => l.Account);
            return View(loanAccounts.ToList());
        }

        // GET: LoanAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccount loanAccount = db.LoanAccounts.Find(id);
            if (loanAccount == null)
            {
                return HttpNotFound();
            }
            return View(loanAccount);
        }

        // GET: LoanAccounts/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID");
            return View();
        }

        // POST: LoanAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanAccountID,AccountID,LoanAmount,LoanTransfered")] LoanAccount loanAccount)
        {
            if (ModelState.IsValid)
            {
                db.LoanAccounts.Add(loanAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", loanAccount.AccountID);
            return View(loanAccount);
        }

        // GET: LoanAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccount loanAccount = db.LoanAccounts.Find(id);
            if (loanAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", loanAccount.AccountID);
            return View(loanAccount);
        }

        // POST: LoanAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanAccountID,AccountID,LoanAmount,LoanTransfered")] LoanAccount loanAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", loanAccount.AccountID);
            return View(loanAccount);
        }

        // GET: LoanAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccount loanAccount = db.LoanAccounts.Find(id);
            if (loanAccount == null)
            {
                return HttpNotFound();
            }
            return View(loanAccount);
        }

        // POST: LoanAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanAccount loanAccount = db.LoanAccounts.Find(id);
            db.LoanAccounts.Remove(loanAccount);
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
