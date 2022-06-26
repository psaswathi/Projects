using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollApp.Models
{
    public class CalculatePayroll
    {
        public int EmployeeCode { get; set; }
        public DateTime MonthSelected { get; set; }
        public decimal TotalHours { get; set; }
    }
}