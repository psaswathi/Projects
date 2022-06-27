using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using PayrollApp.Models;

namespace PayrollApp.Controllers
{
    public class EmployeeListsController : Controller
    {
        private PayrollEntities db = new PayrollEntities();

        public ActionResult CalculatePayroll()
        {
            ViewBag.EmployeeCode = new SelectList(db.EmployeeLists, "EmployeeCode", "EmployeeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculatePayroll(CalculatePayroll payroll)
        {

            if (payroll.TotalHours <= 0)
            {
                ModelState.AddModelError("TotalHours", "Total Hours is wrong");
            }
            if (ModelState.IsValid)
            {
                EmployeePayroll employeePayroll = FetchEmployeePayroll(payroll);

                ViewBag.MonthSelected = payroll.MonthSelected;
                return View("EmployeePayroll", employeePayroll);

            }
            ViewBag.EmployeeCode = new SelectList(db.EmployeeLists, "EmployeeCode", "EmployeeName");
            return View(payroll);
        }

        private EmployeePayroll FetchEmployeePayroll(CalculatePayroll payroll)
        {
            var employee = db.EmployeeLists.Find(payroll.EmployeeCode);
            EmployeePayroll employeePayroll = new EmployeePayroll
            {
                EmployeeName = employee.EmployeeName,
                EmployeeCode = payroll.EmployeeCode,
                DateOfJoining = employee.JoiningDate,
                TotalYears = (payroll.MonthSelected - employee.JoiningDate).TotalDays / 365,
                PayrollMonth = payroll.MonthSelected.ToString("MMM") + " " + payroll.MonthSelected.ToString("yyyy")

            };

            //Calculate current wage
            decimal currentHourlyWages = employee.JobTitleList.Salary;

            int totalYearsCompleted = Convert.ToInt32(Math.Floor(employeePayroll.TotalYears));

            for (int i = 0; i < totalYearsCompleted; i++)
            {
                currentHourlyWages += (currentHourlyWages * 0.1m);
            }

            employeePayroll.HourlySalary = currentHourlyWages;
            employeePayroll.NumberOfHoursWorked = payroll.TotalHours;

            decimal totalPay = currentHourlyWages * payroll.TotalHours;
            employeePayroll.BasicPay = totalPay * 0.64m;
            employeePayroll.HousingAllowance = totalPay * 0.24m;
            employeePayroll.TransportAllowance = totalPay * 0.12m;

            if (employeePayroll.BasicPay > 1000)
            {
                employeePayroll.TaxableAmount = employeePayroll.BasicPay - 1000;
                employeePayroll.Tax = employeePayroll.TaxableAmount * 0.30m;
            }
            else
            {
                employeePayroll.TaxableAmount = 0;
                employeePayroll.Tax = 0;
            }

            employeePayroll.TotalPay = employeePayroll.BasicPay +
                employeePayroll.HousingAllowance +
                employeePayroll.TransportAllowance -
                employeePayroll.Tax;

            return employeePayroll;
        }

        [HttpGet]
        public ActionResult DownloadReport(CalculatePayroll payroll)
        {
            EmployeePayroll employeePayroll = FetchEmployeePayroll(payroll);

            ReportDocument report = new ReportDocument();

            var reportData = new List<EmployeePayroll>();
            reportData.Add(employeePayroll);

            report.Load(Path.Combine(Server.MapPath("~/Reports"), "MonthlyPayroll.rpt"));            
            report.SetDataSource(reportData);
            
            Stream s = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var memoryStream = new MemoryStream();
            s.CopyTo(memoryStream);

            byte[] byteContent = memoryStream.ToArray();
            FileResult file = this.File(byteContent, "application/pdf");

            //Crystal Report Generation END-------------------------------------------------------------------
            return file;
        }

        public ActionResult EmployeePayroll(EmployeePayroll employeePayroll)
        {
            if (employeePayroll.EmployeeCode <= 0)
            {
                ViewBag.EmployeeCode = new SelectList(db.EmployeeLists, "EmployeeCode", "EmployeeName");
                return View("CalculatePayroll");
            }
            return View(employeePayroll);
        }

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
        public ActionResult Create(EmployeeList employeeList)
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
        public ActionResult Edit(EmployeeList employeeList)
        {
            if (ModelState.IsValid)
            {
                EmployeeList employee = db.EmployeeLists.Find(employeeList.ID);

                if (employee == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    employee.EmployeeName = employeeList.EmployeeName;
                    employee.JobCode = employeeList.JobCode;
                    employee.JoiningDate = employeeList.JoiningDate;
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
