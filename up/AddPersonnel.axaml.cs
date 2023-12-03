using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using up.Models;
using System;

namespace up;

public partial class AddPersonnel : Window
{
    private MySqlConnectionStringBuilder _connectionSb;
    private List<MedicalPersonnel> MedicalPersonnels { get; set; }
    public AddPersonnel()
    {
        InitializeComponent();
        MedicalPersonnels = new List<MedicalPersonnel>();
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
        string phone = PhoneTextBox.Text;
        string login = LoginTextBox.Text;
        string password = PasswordTextBox.Text;
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO MedicalPersonnel (Name, Surname, Phone, Login, Password) VALUES (@Name, @Surname, @Phone, @Login, @Password)";
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        UpMedicalPersonnels();
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
        }
    }
    
}