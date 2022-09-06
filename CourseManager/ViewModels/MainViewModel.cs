using Caliburn.Micro;
using CourseManager.Models;
using CourseManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.ViewModels
{
    internal class MainViewModel : Screen
    {
        // View binds to data via BindableCollection.
        private BindableCollection<EnrollmentModel> _enrollments = new BindableCollection<EnrollmentModel>();
        private BindableCollection<StudentModel> _students = new BindableCollection<StudentModel>();
        private BindableCollection<CourseModel> _courses = new BindableCollection<CourseModel>();

        private EnrollmentModel _selectedEnrollment;
        private string _appStatus;

        public EnrollmentModel SelectedEnrollment
        {
            get 
            { 
                return _selectedEnrollment; 
            }
            set 
            { 
                _selectedEnrollment = value;
                NotifyOfPropertyChange(() => SelectedEnrollment);
                NotifyOfPropertyChange(() => SelectedEnrollmentCourse);
                NotifyOfPropertyChange(() => SelectedEnrollmentStudent);
            }
        }

        public string AppStatus
        {
            get { return _appStatus; }
            set { _appStatus = value; }
        }

        public BindableCollection<StudentModel> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        public BindableCollection<CourseModel> Courses
        {
            get { return _courses; }
            set { _courses = value; }
        }

        public BindableCollection<EnrollmentModel> Enrollments
        {
            get { return _enrollments; }
            set { _enrollments = value; }
        }

        public CourseModel SelectedEnrollmentCourse
        {
            get
            {
                try
                {
                    var courseDict = _courses.ToDictionary(x => x.CourseId);
                    if (SelectedEnrollment != null && courseDict.ContainsKey(SelectedEnrollment.CourseId))
                    {
                        return courseDict[SelectedEnrollment.CourseId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentCourse = value;

                    SelectedEnrollment.CourseId = selectedEnrollmentCourse.CourseId;

                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        public StudentModel SelectedEnrollmentStudent
        {
            get
            {
                try
                {
                    var studentDict = _students.ToDictionary(x => x.StudentId);
                    if (SelectedEnrollment != null && studentDict.ContainsKey(SelectedEnrollment.StudentId))
                    {
                        return studentDict[SelectedEnrollment.StudentId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentStudent = value;

                    SelectedEnrollment.StudentId = selectedEnrollmentStudent.StudentId;

                    NotifyOfPropertyChange(() => SelectedEnrollment);
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        public MainViewModel()
        {
            try
            {
                _selectedEnrollment = new EnrollmentModel();

                var studentCmd = new StudentCommand();
                _students.AddRange(studentCmd.Get());

                var courseCmd = new CourseCommand();
                _courses.AddRange(courseCmd.Get());

                var enrollmentCmd = new EnrollmentCommand();
                _enrollments.AddRange(enrollmentCmd.Get());
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }

        private void UpdateAppStatus(string message)
        {
            AppStatus = message;
            NotifyOfPropertyChange(() => AppStatus);
        }

        public void CreateNewEnrollment()
        {
            try
            {
                SelectedEnrollment = new EnrollmentModel();
                
                UpdateAppStatus("Enrollment created.");
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }

        public void SaveEnrollment()
        {
            try
            {
                var enrollmentsDict = _enrollments.ToDictionary(x => x.EnrollmentId);

                if (SelectedEnrollment != null)
                {
                    var enrollmentCmd = new EnrollmentCommand();
                    enrollmentCmd.Upsert(SelectedEnrollment);

                    _enrollments.Clear();
                    _enrollments.AddRange(enrollmentCmd.Get());

                    UpdateAppStatus("Enrollment saved.");
                }
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }
    }
}
