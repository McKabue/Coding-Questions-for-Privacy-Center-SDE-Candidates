
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesLibrary
{
    public class Employees
    {
        private readonly (bool, List<EmployeeModel>) ValidatedEmployeesCsvData = (false, null);

        /// <summary>
        /// This receives employees CSV data string and validates it.
        /// </summary>
        /// <param name="employees"></param>
        public Employees(string employees)
        {
            if (employees.HasValue())
            {
                ValidatedEmployeesCsvData = Utils.ValidateEmployeesCsvData(employees);
            }
        }

        /// <summary>
        /// Is true if employee data is is valid.
        /// </summary>
        public bool IsCsvDataValid
        {
            get
            {
                return ValidatedEmployeesCsvData.Item1 &&
                !SomeEmployeesHaveMultipleManagers &&
                !ThereAreSeveralCEO &&
                !ThereAreCircularReferences &&
                !SomeManagersAreNotEmployees;
            }
        }

        /// <summary>
        /// Is true value if some employees have more than one manager.
        /// </summary>
        public bool SomeEmployeesHaveMultipleManagers
        {
            get
            {
                return ValidatedEmployeesCsvData.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.EmployeeId)
                    ?.Any(i =>
                        i.GroupBy(j => j.ManagerId).Count() > 1)
                ?? false;
            }
        }

        /// <summary>
        /// Is true value if there are more that one employees with no managers.
        /// </summary>
        public bool ThereAreSeveralCEO
        {
            get
            {
                return ValidatedEmployeesCsvData.Item2?.Where(i => !i.ManagerId.HasValue())?.Count() > 1;
            }
        }

        /// <summary>
        /// Is true value if there are circular references.
        /// </summary>
        public bool ThereAreCircularReferences
        {
            get
            {
                return ValidatedEmployeesCsvData.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.ManagerId)
                    ?.Any(i =>
                        {
                            string currentManagerId = i.Key;
                            IEnumerable<string> employeesUnderCurrentManager = i?.Select(k => k.EmployeeId);
                            IEnumerable<string> managersOverCurrentManager = ValidatedEmployeesCsvData.Item2?.Where(j => j.EmployeeId == currentManagerId)?.Select(k => k.ManagerId);

                            return employeesUnderCurrentManager?.Intersect(managersOverCurrentManager)?.Any() ?? false;
                        })

                ?? false;
            }
        }

        /// <summary>
        /// Is true value if some managers are not employees.
        /// </summary>
        public bool SomeManagersAreNotEmployees
        {
            get
            {
                return ValidatedEmployeesCsvData.Item2
                    ?.Where(i => i.ManagerId.HasValue())
                    ?.GroupBy(i => i.ManagerId)
                    ?.Any(i => !ValidatedEmployeesCsvData.Item2.Any(j => j.EmployeeId == i.Key))

                ?? false;
            }
        }

        /// <summary>
        /// The salary budget from a manager is defined as the sum of the salaries of all the employees 
        /// reporting(directly or indirectly) to a specified manager, plus the salary of the manager.
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns>salary budget from the specified manager</returns>
        public int SalaryBudget(string managerId)
        {
            int salaryBudget(string employeeId)
            {
                IEnumerable<EmployeeModel> employees = ValidatedEmployeesCsvData.Item2.Where(i => i.ManagerId == employeeId);
                return (employees?.Sum(i => salaryBudget(i.EmployeeId)) ?? 0) + (employees?.Sum(i => i.Salary) ?? 0);
            }

            EmployeeModel employee = ValidatedEmployeesCsvData.Item2.FirstOrDefault(i => i.EmployeeId == managerId);
            return salaryBudget(employee?.EmployeeId) + (employee?.Salary ?? 0);
        }
    }
}
