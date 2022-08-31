using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class StudentCommand
    {
        private string _connstr;

        public StudentCommand()
        {
            _connstr = CourseReportConnectionString.ConnectionString;
        }

        public IList<StudentModel> Get()
        {
            var courses = new List<StudentModel>();

            using (var conn = new SqlConnection(_connstr))
            {
                string sql = "Students_Get";

                courses = conn.Query<StudentModel>(sql).ToList();
            }

            return courses;
        }
    }
}
