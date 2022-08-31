using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager
{
    public static class CourseReportConnectionString
    {
        public static string ConnectionString => 
            @"Data Source=ITALY\SQLEXPRESS;Initial Catalog=CourseReport;TrustServerCertificate=True;Integrated Security=True";
    }
}
