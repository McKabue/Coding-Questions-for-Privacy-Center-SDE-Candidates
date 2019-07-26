# Linux Ubuntu Setup using Visual Studio Code
1. Create folder called **Employees** and inside **Employees** directory, create a .Net Core solution. 

   `mkdir Employees`

   `cd Employees`

   `dotnet new sln`

2. Create a directory called **EmployeesLibrary** directory, inside  **Employees** directory, and inside **EmployeesLibrary** create a .Net Core Class library.

   `mkdir EmployeesLibrary`

   `cd EmployeesLibrary`

   `dotnet new classlib`

3. Go back to  **Employees** directory and add the **EmployeesLibrary** Class library to the solution.

   `cd ..`

   `dotnet sln add ./EmployeesLibrary/EmployeesLibrary.csproj`

4. Create a unit test directory in  **Employees** directory called **EmployeesLibrary.Tests**, and inside **EmployeesLibrary.Tests** directory, create a Unit Test Project.

   `mkdir EmployeesLibrary.Tests`

   `cd EmployeesLibrary.Tests`

   `dotnet new xunit`

5. Add **EmployeesLibrary** Class Library to **EmployeesLibrary.Tests** Unit Test Project as a dependency.

   `dotnet add reference ../EmployeesLibrary/EmployeesLibrary.csproj`

6. Go back to  **Employees** directory and add the **EmployeesLibrary.Tests** Unit Test Project to the solution.

   `cd ..`

   `dotnet sln add ./EmployeesLibrary.Tests/EmployeesLibrary.Tests.csproj`

7. Running Unit tests.
    go to **EmployeesLibrary.Tests** directory.
   `cd EmployeesLibrary.Tests`

   To run all tests, execute:
   `dotnet test`

   To generate a test log file, run:
   `dotnet test --logger:trx`
