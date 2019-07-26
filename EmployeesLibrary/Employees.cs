
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesLibrary
{
    public class Employees
    {
        private (bool, List<EmployeeModel>) IsCSVDataValid = (false, null);

        /// <summary>
        /// This receives an employees info CSV string and validates it.
        /// </summary>
        /// <param name="employees"></param>
        public Employees(string employees)
        {
            if (employees.HasValue())
            {
                IsCSVDataValid = Utils.IsCSVDataValid(employees);
            }
        }

        public bool IsValidCSV { get => IsCSVDataValid.Item1; }

        public bool DoesAnyEmployeeHaveMultipleManagers
        {
            get
            {
                return IsCSVDataValid.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.EmployeeId)
                    ?.Any(i =>
                        i.GroupBy(j => j.ManagerId).Count() > 1)
                ?? false;
            }
        }

        public bool ThereAreSeveralCEO
        {
            get
            {
                return IsCSVDataValid.Item2?.Where(i => !i.ManagerId.HasValue())?.Count() > 1;
            }
        }

        public bool ThereIsCircularReference
        {
            get
            {
                return IsCSVDataValid.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.ManagerId)
                    ?.Any(i =>
                        {
                            string currentManagerId = i.Key;
                            IEnumerable<string> employeesUnderCurrentManager = i?.Select(k => k.EmployeeId);
                            IEnumerable<string> managersOverCurrentManager = IsCSVDataValid.Item2?.Where(j => j.EmployeeId == currentManagerId)?.Select(k => k.ManagerId);

                            return employeesUnderCurrentManager?.Intersect(managersOverCurrentManager)?.Any() ?? false;
                        })

                ?? false;
            }
        }

        public bool SomeManagersAreNotEmployees
        {
            get
            {
                return IsCSVDataValid.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.ManagerId)
                    ?.Any(i => IsCSVDataValid.Item2?.Any(j => j.EmployeeId == i.Key) ?? false)

                ?? false;
            }
        }

        public int SalaryBudget(string managerId)
        {
            int salaryBudget(string _managerId, int salary = 0)
            {
                var employees = IsCSVDataValid.Item2.Where(i => i.ManagerId == _managerId);
                return (employees?.Sum(i => salaryBudget(i.EmployeeId)) ?? 0) + (employees?.Sum(i => i.Salary) ?? 0);
            }

            var employee = IsCSVDataValid.Item2.FirstOrDefault(i => i.EmployeeId == managerId);
            return salaryBudget(employee?.EmployeeId) + (employee?.Salary ?? 0);
        }
    }
}
