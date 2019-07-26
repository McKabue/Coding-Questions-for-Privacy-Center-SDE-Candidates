using System;
using Xunit;

namespace EmployeesLibrary.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            Employees employees = new Employees("");

            Assert.False(employees.IsValidCSV, "CVC is not Valid");
        }

        [Theory]
        [InlineData(@"
                EmployeeId,ManagerId,Salary")]
        [InlineData(@"
                EmployeeId,ManagerId,Salary
                1,dsd,2341")]
        [InlineData(@"
                1,dsd,2341")]
        public void Test2(string csvData)
        {
            Employees employees = new Employees(csvData);

            Assert.True(employees.IsValidCSV, "CVC is Valid");
        }

        [Theory]
        [InlineData(@"
                EmployeeId,ManagerId,Salary
                1,dsd,sss")]
        public void Test3(string csvData)
        {
            Employees employees = new Employees(csvData);

            Assert.False(employees.IsValidCSV, "Salary needs to be an integer");
        }

        [Fact]
        public void Test4()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                1,ddd,12
                1,ddd,12");

            Assert.False(employees.DoesAnyEmployeeHaveMultipleManagers, "All employees have one manager");
        }

        [Fact]
        public void Test5()
        {
            Employees employees2 = new Employees(@"
                EmployeeId,ManagerId,Salary
                1,dsd,12
                1,eee,12");

            Assert.True(employees2.DoesAnyEmployeeHaveMultipleManagers, "Some employees have several managers");
        }

        [Fact]
        public void Test6()
        {
            Employees employees2 = new Employees(@"
                EmployeeId,ManagerId,Salary
                1,,12
                1,,12");

            Assert.True(employees2.ThereAreSeveralCEO, "There are several CEO");
        }

        [Fact]
        public void Test7()
        {
            Employees employees1 = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,manager1,12
                manager1,employee1,12");

            Assert.True(employees1.ThereIsCircularReference, "There is circular reference");

            Employees employees2 = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,manager1,12
                employee2,employee1,12");

            Assert.False(employees2.ThereIsCircularReference, "There is no circular reference");
        }

        [Fact]
        public void Test8()
        {
            Employees employees1 = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,manager1,12
                employee2,employee1,12");

            Assert.True(employees1.SomeManagersAreNotEmployees, "Some managers are not employees");
        }


        [Fact]
        public void Test9()
        {
            Employees employees = new Employees(@"
                EmployeeId,ManagerId,Salary
                employee1,,12

                employee2,employee1,12
                employee3,employee1,12,
                employee4,employee1,12
                
                employee5,employee2,12
                employee6,employee2,12,
                employee7,employee2,12
                
                employee8,employee5,12
                employee9,employee5,12,
                employee10,employee5,12
                
                employee11,employee8,12
                employee12,employee8,12,
                employee13,employee8,12");

            Assert.Equal(156, employees.SalaryBudget(null));

            Assert.Equal(120, employees.SalaryBudget("employee2"));

            Assert.Equal(84, employees.SalaryBudget("employee5"));

            Assert.Equal(48, employees.SalaryBudget("employee8"));
        }
    }
}
