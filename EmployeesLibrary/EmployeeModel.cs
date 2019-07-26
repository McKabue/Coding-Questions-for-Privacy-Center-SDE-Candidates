using System.ComponentModel.DataAnnotations;

namespace EmployeesLibrary
{
    public class EmployeeModel
    {
        private string _id;
        private string _manager;

        [Required(ErrorMessage = "Employee Id can't be null")]
        public string EmployeeId
        {
            get
            {
                return _id.HasValue() ? _id.Trim().Normalize() : null;
            }
            set => _id = value;
        }

        public string ManagerId
        {
            get
            {
                return _manager.HasValue() ? _manager.Trim().Normalize() : null;
            }
            set => _manager = value;
        }

        [Required(ErrorMessage = "All employees must have a salary")]
        public int Salary { get; set; }
    }
}
