namespace CourseManager.Models
{
    internal class EnrollmentModel
    {
        public string EnrollmentId { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public bool IsCommitted { get; set; }
    }
}
