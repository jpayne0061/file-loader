using System;
using System.Collections.Generic;
using System.Text;

namespace fileLoader
{
    public class Record
    {
        public int SalaryDataID { get; set; }
        public int CalendarYear { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public float AnnualRate { get; set; }
        public float RegularRate { get; set; }
        public float OvertimeRate { get; set; }
        public float IncentiveAllowance { get; set; }
        public float Other { get; set; }
        public float YearToDate { get; set; }

    }
}

