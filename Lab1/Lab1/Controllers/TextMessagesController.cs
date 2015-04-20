using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab1.Models;

namespace Lab1.Controllers
{
    public class TextMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TextMessages
        public ActionResult Index()
        {
            return View(db.TextMessages.ToList());
        }

        // GET: TextMessages/Details/5
        public ActionResult Details(TextMessage textMessage)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //TextMessage textMessage = db.TextMessages.Find(id);
            if (textMessage == null)
            {
                return HttpNotFound();
            }
            return View(textMessage);
        }

        // GET: TextMessages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TextID,ToMobileNumber,Content")] TextMessage textMessage)
        {
            if (ModelState.IsValid)
            {
                db.TextMessages.Add(textMessage);
                db.SaveChanges();
                return RedirectToAction("Details", textMessage);
            }
            else
            {
                return View();
            }

            
            //return View(textMessage);
        }

        // GET: TextMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextMessage textMessage = db.TextMessages.Find(id);
            if (textMessage == null)
            {
                return HttpNotFound();
            }
            return View(textMessage);
        }

        // POST: TextMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TextID,ToMobileNumber,Content")] TextMessage textMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(textMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textMessage);
        }

        // GET: TextMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextMessage textMessage = db.TextMessages.Find(id);
            if (textMessage == null)
            {
                return HttpNotFound();
            }
            return View(textMessage);
        }

        // POST: TextMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TextMessage textMessage = db.TextMessages.Find(id);
            db.TextMessages.Remove(textMessage);
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
