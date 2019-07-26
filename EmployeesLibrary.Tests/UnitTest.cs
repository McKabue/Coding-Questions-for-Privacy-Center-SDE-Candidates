using System;
using Xunit;

namespace EmployeesLibrary.Tests
{
    public class UnitTest
    {
        /// <summary>
        /// Validates the format of Employees CSV data.
        /// </summary>
        [Fact]
        public void ValidateEmployeesCsvData()
        {
            Employees employees1 = new Employees("");
            Assert.False(employees1.IsCsvDataValid, "An empty string CSV data is invalid");

            Employees employees2 = new Employees(@"
                EmployeeId,ManagerId,Salary");
            Assert.True(employees2.IsCsvDataValid, "CSV data with just header is valid");

            Employees employees3 = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000
                employee2,employee1,250000");
            Assert.True(employees3.IsCsvDataValid, "CSV data with header and rows of employees is valid");

            Employees employees4 = new Employees(@"
                employee1,,500000");
            Assert.True(employees4.IsCsvDataValid, "CSV data rows of employees is valid");
        }

        /// <summary>
        /// Validates that salaries in the CSV are valid integer numbers.
        /// </summary>
        [Fact]
        public void ValidateEmployeeSalary()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,half a million");

            Assert.False(employees.IsCsvDataValid, "Employee Salary needs to be an integer");
        }

        /// <summary>
        /// Validates that one employee does not report to more than one manager.
        /// </summary>
        [Fact]
        public void ValidatesEmployeeManager()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000
                employee2,employee1,250000
                employee3,employee1,150000
                employee3,employee2,150000");

            Assert.False(employees.IsCsvDataValid, "An Employee can not have multiple managers.");
        }

        /// <summary>
        /// Validates that there is only one CEO, i.e. only one employee with no manager.
        /// </summary>
        [Fact]
        public void ValidatesThereIsOnlyOneCEO()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000
                employee2,,500000");

            Assert.False(employees.IsCsvDataValid, "There should only one employee with no manager.");
        }

        /// <summary>
        /// Validates that there is no circular reference, 
        /// i.e. a first employee reporting to a second employee that is also under the first employee.
        /// </summary>
        [Fact]
        public void ValidatesCircularReference()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000
                employee2,employee3,250000
                employee3,employee2,150000");

            Assert.False(employees.IsCsvDataValid, "There are circular references.");
        }


        /// <summary>
        /// Validates there is no manager that is not an employee, 
        /// i.e. all managers are also listed in the employee column.
        /// </summary>
        [Fact]
        public void ValidatesAllManagersAreEmployees()
        {
            Employees employees1 = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000
                employee2,employee3,250000");

            Assert.False(employees1.IsCsvDataValid, "All managers should be employees");
        }


        [Fact]
        public void ValidatesSalaryBudget()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,500000

                employee2,employee1,400000
                employee3,employee1,400000
                employee4,employee1,400000
                
                employee5,employee2,300000
                employee6,employee2,300000
                employee7,employee2,300000
                
                employee8,employee5,200000
                employee9,employee5,200000
                employee10,employee5,200000
                
                employee11,employee8,100000
                employee12,employee8,100000
                employee13,employee8,100000");

            Assert.True(employees.IsCsvDataValid, "Employees data is valid");

            Assert.Equal(3500000, employees.SalaryBudget(null));

            Assert.Equal(2200000, employees.SalaryBudget("employee2"));

            Assert.Equal(1200000, employees.SalaryBudget("employee5"));

            Assert.Equal(500000, employees.SalaryBudget("employee8"));
        }
    }
}
