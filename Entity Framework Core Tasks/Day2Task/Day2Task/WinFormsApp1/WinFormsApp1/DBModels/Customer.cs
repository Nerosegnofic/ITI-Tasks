using System;
using System.Collections.Generic;

namespace WinFormsApp1.DBModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }
}
