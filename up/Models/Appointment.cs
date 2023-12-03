namespace up.Models;

public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int DiseaseId { get; set; }
    public int PaymentId { get; set; }
    public bool Attendance { get; set; }
    public int TreatmentId { get; set; }
    public int MedicalCard { get; set; }
    public MedicalPersonnel Doctor { get; set; }
    public Disease Disease { get; set; }
    public Payment Payment { get; set; }
    public Treatment Treatment { get; set; }
}