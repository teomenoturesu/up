using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using up.Models;
using System;

namespace up;

public partial class AddPatient : Window
{
    private MySqlConnectionStringBuilder _connectionSb;
    private List<Patient> Patients { get; set; }
    public AddPatient()
    {
        InitializeComponent();
        Patients = new List<Patient>();
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "Up",
            UserID = "root",
            Password = "11111"
        };
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text;
        string surname = SurnameTextBox.Text;
        DateTimeOffset dateOfBirth = DateOfBirthPicker.SelectedDate ?? DateTime.MinValue;
        string phone = PhoneTextBox.Text;
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Patient (Name, Surname, DateOfBirth, Phone) VALUES (@Name, @Surname, @DateOfBirth, @Phone)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@Phone", phone);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        UpPatiens();
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
    }
}