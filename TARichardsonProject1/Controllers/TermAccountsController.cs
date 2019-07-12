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
    public class TermAccountsController : Controller
    {
        private OnlineBankingDBEntities db = new OnlineBankingDBEntities();

        // GET: TermAccounts
        public ActionResult Index()
        {
            var termAccounts = db.TermAccounts.Include(t => t.Account);
            return View(termAccounts.ToList());
        }

        // GET: TermAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermAccount termAccount = db.TermAccounts.Find(id);
            if (termAccount == null)
            {
                return HttpNotFound();
            }
            return View(termAccount);
        }

        // GET: TermAccounts/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID");
            return View();
        }

        // POST: TermAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TermAccountID,AccountID,DateOfMaturity")] TermAccount termAccount)
        {
            if (ModelState.IsValid)
            {
                db.TermAccounts.Add(termAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", termAccount.AccountID);
            return View(termAccount);
        }

        // GET: TermAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermAccount termAccount = db.TermAccounts.Find(id);
            if (termAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", termAccount.AccountID);
            return View(termAccount);
        }

        // POST: TermAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TermAccountID,AccountID,DateOfMaturity")] TermAccount termAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(termAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "AccountID", termAccount.AccountID);
            return View(termAccount);
        }

        // GET: TermAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermAccount termAccount = db.TermAccounts.Find(id);
            if (termAccount == null)
            {
                return HttpNotFound();
            }
            return View(termAccount);
        }

        // POST: TermAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TermAccount termAccount = db.TermAccounts.Find(id);
            db.TermAccounts.Remove(termAccount);
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
