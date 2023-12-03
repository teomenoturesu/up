namespace up.Models;

public class MedicalCard
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
}