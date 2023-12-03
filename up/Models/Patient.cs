using System;

namespace up.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string Phone { get; set; }
}