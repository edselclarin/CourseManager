using CourseManager.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CourseManager.Repository
{
    internal class EnrollmentCommand
    {
        private string _connstr;

        public EnrollmentCommand()
        {
            _connstr = CourseReportConnectionString.ConnectionString;
        }

        public IList<EnrollmentModel> Get()
        {
            var enrollments = new List<EnrollmentModel>();

            using (var conn = new SqlConnection(_connstr))
            {
                string sql = "Enrollments_Get";

                enrollments = conn.Query<EnrollmentModel>(sql).ToList();
            }

            foreach (var enrollment in enrollments)
            {
                enrollment.IsCommitted = true;
            }

            return enrollments;
        }

        public void Upsert(EnrollmentModel model)
        {
            string userId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            var dt = new DataTable();
            dt.Columns.Add("EnrollmentId", typeof(int));
            dt.Columns.Add("StudentId", typeof(int));
            dt.Columns.Add("CourseId", typeof(int));
            dt.Rows.Add(model.EnrollmentId, model.StudentId, model.CourseId);

            using (var conn = new SqlConnection(_connstr))
            {
                string sql = "Enrollments_Upsert";

                conn.Execute(
                    sql, 
                    new 
                    {
                        @EnrollmentType = dt.AsTableValuedParameter("EnrollmentType"),
                        @UserId = userId
                    }, 
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
