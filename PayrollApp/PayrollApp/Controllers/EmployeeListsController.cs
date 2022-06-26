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
    public class EmployeeListsController : Controller
    {
        private PayrollEntities db = new PayrollEntities();

        // GET: EmployeeLists
        public ActionResult Index()
        {
            var employeeLists = db.EmployeeLists.Include(e => e.JobTitleList);
            return View(employeeLists.ToList());
        }

        // GET: EmployeeLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeList employeeList = db.EmployeeLists.Find(id);
            if (employeeList == null)
            {
                return HttpNotFound();
            }
            return View(employeeList);
        }

        // GET: EmployeeLists/Create
        public ActionResult Create()
        {
            ViewBag.JobCode = new SelectList(db.JobTitleLists, "JobCode", "JobName");
            return View();
        }

        // POST: EmployeeLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeCode,EmployeeName,JobCode,JoiningDate,IsActive,CreatedDate,ModifiedDate")] EmployeeList employeeList)
        {
            DateTime time = DateTime.Now;
            if (ModelState.IsValid)
            {
                var lastEmpID = db.EmployeeLists.OrderByDescending(i => i.ID).FirstOrDefault();
                if (lastEmpID == null)
                {
                    employeeList.EmployeeCode = 100000 + 1;
                }
                else
                {
                    employeeList.EmployeeCode = 100000 + lastEmpID.ID + 1;
                }

                employeeList.CreatedDate = time;
                employeeList.ModifiedDate = time;
                db.EmployeeLists.Add(employeeList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobCode = new SelectList(db.JobTitleLists, "JobCode", "JobName", employeeList.JobCode);
            return View(employeeList);
        }

        // GET: EmployeeLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeList employeeList = db.EmployeeLists.Find(id);
            if (employeeList == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobCode = new SelectList(db.JobTitleLists, "JobCode", "JobName", employeeList.JobCode);
            return View(employeeList);
        }

        // POST: EmployeeLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeCode,EmployeeName,JobCode,JoiningDate,IsActive,CreatedDate,ModifiedDate")] EmployeeList employeeList)
        {
            if (ModelState.IsValid)
            {
                EmployeeList employee=db.EmployeeLists.Find(employeeList.ID);

                if(employee == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    employee.EmployeeName = employeeList.EmployeeName;
                    employee.JobCode= employeeList.JobCode;
                    employee.JoiningDate = employeeList.JoiningDate;
                    employee.IsActive = employeeList.IsActive;
                    employee.ModifiedDate = DateTime.Now;
                }

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobCode = new SelectList(db.JobTitleLists, "JobCode", "JobName", employeeList.JobCode);
            return View(employeeList);
        }

        // GET: EmployeeLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeList employeeList = db.EmployeeLists.Find(id);
            if (employeeList == null)
            {
                return HttpNotFound();
            }
            return View(employeeList);
        }

        // POST: EmployeeLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeList employeeList = db.EmployeeLists.Find(id);
            db.EmployeeLists.Remove(employeeList);
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
