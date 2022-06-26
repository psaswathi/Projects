using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayrollApp.Models;

namespace PayrollApp.Controllers
{
    public class JobTitleListsController : Controller
    {
        private PayrollEntities db = new PayrollEntities();

        // GET: JobTitleLists
        public ActionResult Index()
        {
            return View(db.JobTitleLists.ToList());
        }

        // GET: JobTitleLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitleList jobTitleList = db.JobTitleLists.Find(id);
            if (jobTitleList == null)
            {
                return HttpNotFound();
            }
            return View(jobTitleList);
        }

        // GET: JobTitleLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobTitleLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobTitleList jobTitleList)
        {
            DateTime time = DateTime.Now;
            if (ModelState.IsValid)
            {
                var job = db.JobTitleLists.OrderByDescending(i => i.ID).FirstOrDefault();
                if(job == null)
                {
                    jobTitleList.JobCode = 100+1;
                }
                else
                {
                    jobTitleList.JobCode = 100+job.ID+1;
                }
                
                jobTitleList.CreatedDate = time;
                jobTitleList.ModifiedDate = time;
                
                db.JobTitleLists.Add(jobTitleList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobTitleList);
        }

        // GET: JobTitleLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitleList jobTitleList = db.JobTitleLists.Find(id);
            if (jobTitleList == null)
            {
                return HttpNotFound();
            }
            return View(jobTitleList);
        }

        // POST: JobTitleLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobTitleList jobTitleList)
        {
            DateTime time = DateTime.Now;
            if (ModelState.IsValid)
            {
                JobTitleList jobTitle = db.JobTitleLists.Find(jobTitleList.ID);
                jobTitle.JobName = jobTitleList.JobName;
                jobTitle.JobDesc= jobTitleList.JobDesc;
                jobTitle.Salary= jobTitleList.Salary;
                jobTitle.ModifiedDate = time;
                db.Entry(jobTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobTitleList);
        }

        // GET: JobTitleLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTitleList jobTitleList = db.JobTitleLists.Find(id);
            if (jobTitleList == null)
            {
                return HttpNotFound();
            }
            return View(jobTitleList);
        }

        // POST: JobTitleLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobTitleList jobTitleList = db.JobTitleLists.Find(id);
            db.JobTitleLists.Remove(jobTitleList);
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
