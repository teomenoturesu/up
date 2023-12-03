using Avalonia.Automation.Provider;

namespace up.Models;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
}