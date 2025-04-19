using System;
using System.Collections.Generic;

namespace MyMvcApp.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string? StreetAddress { get; set; }

    public string? PostalCode { get; set; }

    public string City { get; set; } = null!;

    public string? StateProvince { get; set; }

    public string? CountryId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
