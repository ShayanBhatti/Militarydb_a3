using System;
using System.Collections.Generic;

namespace MyMvcApp.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public string JobId { get; set; } = null!;

    public decimal? Salary { get; set; }

    public decimal? CommissionPct { get; set; }

    public int? ManagerId { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual Job Job { get; set; } = null!;

    public virtual ICollection<JobHistory> JobHistories { get; set; } = new List<JobHistory>();

    public virtual Employee? Manager { get; set; }
}
