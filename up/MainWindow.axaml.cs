using Avalonia.Controls;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using Avalonia.Interactivity;
using up.Models;

namespace up;

public partial class MainWindow : Window
{
    private List<Appointment> Appointments { get; set; }
    private List<Disease> Diseases { get; set; }
    private List<MedicalCard> MedicalCards { get; set; }
    private List<MedicalPersonnel> MedicalPersonnels { get; set; }
    private List<Patient> Patients { get; set; }
    private List<Payment> Payments { get; set; }
    private List<Treatment> Treatments { get; set; }
    private MySqlConnectionStringBuilder _connectionSb;

    public MainWindow()
    {
        InitializeComponent();
        Appointments = new List<Appointment>();
        Diseases = new List<Disease>();
        MedicalCards = new List<MedicalCard>();
        MedicalPersonnels = new List<MedicalPersonnel>();
        Patients = new List<Patient>();
        Payments = new List<Payment>();
        Treatments = new List<Treatment>();
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "Up",
            UserID = "root",
            Password = "11111"
        };
        UpAppointments();
        UpDiseases();
        UpMedicalCards();
        UpMedicalPersonnels();
        UpPatiens();
        UpPayments();
        UpTreatments();
        CountPatientsTextBlock.Text = $"Количество пациентов: {Patients.Count}";
    }

    void UpAppointments()
    {
        Appointments.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT " +
                    "   Disease.id AS 'disease_id', " +
                    "MedicalPersonnel.id AS 'doctor_id', " +
                    "   Appointment.*, " +
                    "   MedicalPersonnel.*, " +
                    "   Disease.*, " +
                    "   Payment.*, " +
                    "   Treatment.* " +
                    "FROM Appointment " +
                    "JOIN MedicalPersonnel ON Appointment.Doctor = MedicalPersonnel.Id " +
                    "JOIN Disease ON Appointment.Disease = Disease.Id " +
                    "JOIN Payment ON Appointment.Payment = Payment.Id " +
                    "JOIN Treatment ON Appointment.Treatment = Treatment.Id " +
                    "JOIN MedicalCard ON Appointment.MedicalCard = MedicalCard.Id";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointments.Add(new Appointment()
                        {
                            Id = reader.GetInt32("id"),
                            DoctorId = reader.GetInt32("id"),
                            DiseaseId = reader.GetInt32("id"),
                            PaymentId = reader.GetInt32("payment"),
                            Attendance = reader.GetBoolean("attendance"),
                            TreatmentId = reader.GetInt32("id"),
                            MedicalCard = reader.GetInt32("medicalCard"),
                            Doctor = new MedicalPersonnel()
                            {
                                Id = reader.GetInt32("doctor_id"),
                                Name = reader.GetString("name"),
                                Surname = reader.GetString("surname"),
                                Phone = reader.GetString("phone"),
                                Login = reader.GetString("login"),
                                Password = reader.GetString("password")
                            },
                            Disease = new Disease()
                            {
                                Id = reader.GetInt32("disease_id"),
                                Name = reader.GetString("name")
                            },
                            Payment = new Payment()
                            {
                                Id = reader.GetInt32("id"),
                                Amount = reader.GetDecimal("amount"),
                                Status = reader.GetBoolean("status")
                            },
                            Treatment = new Treatment()
                            {
                                Id = reader.GetInt32("id"),
                                Description = reader.GetString("description")
                            }
                        });
                    }
                }
            }

            connection.Close();
        }
        AppointmentDataGrid.ItemsSource = Appointments;
    }

    void UpDiseases()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM Disease";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Diseases.Add(new Disease()
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name")
                        });
                    }
                }
            }

            connection.Close();
        }
        DiseaseDataGrid.ItemsSource = Diseases;
    }

    void UpMedicalCards()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM MedicalCard " +
                    "JOIN Patient ON MedicalCard.Patient = Patient.Id";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicalCards.Add(new MedicalCard()
                        {
                            Id = reader.GetInt32("Id"),
                            PatientId = reader.GetInt32("Id")
                        });
                    }
                }
                connection.Close();
            }
        }
    }

    void UpMedicalPersonnels()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM MedicalPersonnel";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicalPersonnels.Add(new MedicalPersonnel()
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Phone = reader.GetString("Phone"),
                            Login = reader.GetString("Login"),
                            Password = reader.GetString("Password")
                        });
                    }
                }
                connection.Close();
            }
            MedicalPersonnelDataGrid.ItemsSource = MedicalPersonnels;
        }
    }
    
    void UpPatiens()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM Patient";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patients.Add(new Patient()
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            DateOfBirth = reader.GetDateTime("DateOfBirth"),
                            Phone = reader.GetString("Phone")
                        });
                    }    
                }
            }

            connection.Close();
        }
        PatientDataGrid.ItemsSource = Patients;
    }

    void UpPayments()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = 
                    "SELECT * FROM Payment";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Payments.Add(new Payment()
                        {
                            Id = reader.GetInt32("Id"),
                            Amount = reader.GetDecimal("Amount"),
                            Status = reader.GetBoolean("Status")
                        });
                    }
                }
                connection.Close();
            }
        }
    }

    void UpTreatments()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM Treatment";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Treatments.Add(new Treatment()
                        {
                            Id = reader.GetInt32("Id"),
                            Description = reader.GetString("Description")
                        });
                    }
                }

                connection.Close();
            }
        }
    }

    private void ButtonP_OnClick(object? sender, RoutedEventArgs e)
    {
        AddPatient addPatient = new AddPatient();
        addPatient.Show();
    }

    private void ButtonM_OnClick(object? sender, RoutedEventArgs e)
    {
        AddPersonnel addPersonnel = new AddPersonnel();
        addPersonnel.Show();
    }
}