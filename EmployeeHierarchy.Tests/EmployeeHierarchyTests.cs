using System;
using Xunit;
using Xunit.Abstractions;

namespace EmployeeHierarchy.Tests
{
    public class EmployeeHierarchyTests
    {
        public const string EmployeeCsvData = "Employee4,Employee2,500\r\n" +
                                              "Employee3,Employee1,500\r\n" +
                                              "Employee1,,1000\r\n" +
                                              "Employee5,Employee1,500\r\n" +
                                              "Employee2,Employee1,800\r\n" +
                                              "Employee6,Employee2,500";

        public const string EmployeeNonIntegerSalary = "Employee4,Employee2,500\r\n" +
                                                       "Employee3,Employee1,500\r\n" +
                                                       "Employee1,,1000.90\r\n" +
                                                       "Employee5,Employee1,500\r\n" +
                                                       "Employee2,Employee1,800\r\n" +
                                                       "Employee6,Employee2,500.09";

        public const string EmployeeDuplicateCeo = "Employee4,Employee2, 500\r\n" +
                                                   "Employee3,Employee1,500\r\n" +
                                                   "Employee1,,1000\r\n" +
                                                   "Employee5,Employee1,500\r\n" +
                                                   "Employee2,Employee1,800\r\n" +
                                                   "Employee6,Employee2,500\r\n" +
                                                   "Employee9,,500";

        private readonly Employees _employeeHierarchy;
        private readonly ITestOutputHelper _helper;

        public EmployeeHierarchyTests(ITestOutputHelper helper)
        {
            _employeeHierarchy = new Employees(EmployeeCsvData);
            _helper = helper;
        }

        [Theory]
        [InlineData(EmployeeCsvData)]
        public void CanCreateHierachy(string employeesCsv)
        {
            var employeeHierachy = new Employees(employeesCsv);
            Assert.NotNull(employeeHierachy);
            Assert.True(employeeHierachy.Members.Count == 6);
            _helper.WriteLine($"Employee Count: {employeeHierachy.Members.Count}");
        }

        [Theory]
        [InlineData("Employee1", 3800)]
        [InlineData("Employee2", 1800)]
        [InlineData("Employee3", 500)]
        public void CanCalculateBudget(string employeeId, long salary)
        {
            var salaries = _employeeHierarchy.CalculateBudget(employeeId);
            Assert.True(salaries != 0);
            Assert.True(salaries == salary);
            _helper.WriteLine($"EmployeeId: {employeeId} Budget: {salaries}");
        }

        [Theory]
        [InlineData(EmployeeNonIntegerSalary)]
        public void SalariesShouldBeIntegers(string employeesCsv)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var employeesHierarchy = new Employees(employeesCsv);
            });
            _helper.WriteLine(exception.Message);
        }

        [Theory]
        [InlineData(EmployeeDuplicateCeo)]
        public void ShouldOnlyBeOneCeo(string employeesCsv)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var employeesHierarchy = new Employees(employeesCsv);
            });
            _helper.WriteLine(exception.Message);
        }
    }
}