using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class CourseCommand
    {
        private string _connstr;

        public CourseCommand()
        {
            _connstr = CourseReportConnectionString.ConnectionString;
        }

        public IList<CourseModel> Get()
        {
            var courses = new List<CourseModel>();

            using (var conn = new SqlConnection(_connstr))
            {
                string sql = "Courses_Get";

                courses = conn.Query<CourseModel>(sql).ToList();
            }

            return courses;
        }
    }
}
