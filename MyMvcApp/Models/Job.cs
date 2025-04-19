using System;
using System.Collections.Generic;

namespace MyMvcApp.Models;

public partial class Job
{
    public string JobId { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<JobHistory> JobHistories { get; set; } = new List<JobHistory>();
}
