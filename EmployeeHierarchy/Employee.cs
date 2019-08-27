using System.Collections.Generic;
using System.Linq;

namespace EmployeeHierarchy
{
    public class Employee
    {
        private readonly string _managerString;

        public Employee(Employees parentHierarchy, string employeeIdentifier, long salary, string manager = null)
        {
            ParentHierarchy = parentHierarchy;
            EmployeeIdentifier = employeeIdentifier;
            _managerString = manager;
            Salary = salary;
        }

        /// <summary>
        ///     The manager (also an employee) who manager (i.e who this employee) reports to
        /// </summary>
        public Employee Manager
        {
            get
            {
                var managerEmployee =
                    ParentHierarchy.Members.FirstOrDefault(c => c.EmployeeIdentifier == _managerString);
                return managerEmployee;
            }
        }

        /// <summary>
        ///     All the employees who directly report to this manager
        /// </summary>
        public List<Employee> DirectReports
        {
            get
            {
                var employees = ParentHierarchy.Members
                    .Where(c => c.Manager != null && c.Manager.EmployeeIdentifier == EmployeeIdentifier)
                    .ToList();
                return employees;
            }
        }

        /// <summary>
        ///     The Salary of this Employee
        /// </summary>
        public long Salary { get; }

        public Employees ParentHierarchy { get; }

        /// <summary>
        ///     Unique Identifier of this Employee
        /// </summary>
        public string EmployeeIdentifier { get; }
    }
}