using SilverLeaf.Common.Interfaces;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentAPI _studentApi;

        private StudentDTO _user;

        public StudentService(IStudentAPI studentApi)
        {
            _studentApi = studentApi;
        }

        public event EventHandler<StudentDTO> StudentChanged;


        public StudentDTO Student
        {
            get => _user ?? (_user = CacheService.Student);
            set
            {
                if (_user == value)
                {
                    return;
                }

                _user = value;
                CacheStudent();
            }
        }

        public async Task Register(StudentDTO student)
        {
            var response = await _studentApi.Register<StudentDTO>(student).ConfigureAwait(false);
            Student = response.Content;
            CacheService.Student = Student;
        }

        public void ClearStudentCache()
        {
            _user = null;
            CacheService.RemoveStudentSettings();
        }

        private void CacheStudent()
        {
            CacheService.Student = _user;
            StudentChanged?.Invoke(this, _user);
        }
    }
}
