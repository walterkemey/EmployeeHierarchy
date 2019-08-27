using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeHierarchy
{
    public class Employees
    {
        private readonly string _rawData;

        public Employees(string employeesCsv)
        {
            _rawData = employeesCsv;
            Members = new List<Employee>();

            var rows = _rawData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var count = 0;
            do
            {
                var components = rows[count].Split(",");
                var parseResults = long.TryParse(components[2], out var salary);
                if (parseResults == false)
                    throw new ArgumentException($"Employee Salary is not an integer value:{components[2]}",
                        nameof(employeesCsv));
                var employee = new Employee(this, components[0], salary, components[1]);
                Members.Add(employee);
                count++;
            } while (count < rows.Length);

            if (Members.Count(c => c.Manager == null) > 1)
                throw new ArgumentException("More than one CEO", nameof(employeesCsv));
        }

        /// <summary>
        ///     All Employees who are part of this hierarchy
        /// </summary>
        public List<Employee> Members { get; set; }

        /// <summary>
        ///     Calculates the budge of the manager identified by the ID
        /// </summary>
        /// <param name="employeeIdentifier"></param>
        /// <returns></returns>
        public long CalculateBudget(string employeeIdentifier)
        {
            var manager = Members.FirstOrDefault(c => c.EmployeeIdentifier == employeeIdentifier);
            if (manager == null)
                throw new ArgumentException($"Employee with Id: {employeeIdentifier} could not be found",
                    nameof(employeeIdentifier));

            var currentManager = manager;
            var subReportBalance = currentManager.Salary;
            foreach (var employee in currentManager.DirectReports)
            {
                subReportBalance += employee.DirectReports.Sum(c => c.Salary) + employee.Salary;
                if (employee.DirectReports.Count == 0)
                    continue;
                currentManager = employee;
            }

            return subReportBalance;
        }
    }
}