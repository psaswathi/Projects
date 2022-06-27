using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayrollApp.Models
{
    public class EmployeePayroll
    {
        public string EmployeeName { get;set; }
        public int EmployeeCode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateOfJoining { get; set; }
        public double TotalYears { get; set; }
        public string PayrollMonth { get; set; }
        public int NumberOfHoursWorked { get; set; }
        public decimal HourlySalary { get; set; }
        public decimal BasicPay { get; set; }
        public decimal HousingAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPay { get; set; }

    }
}