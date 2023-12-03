using Avalonia.Controls;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using Avalonia.Interactivity;
using up.Models;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace up;

public partial class Authorization : Window
{
    private MySqlConnectionStringBuilder _connectionSb;
    private List<MedicalPersonnel> MedicalPersonnels { get; set; }
    public Authorization()
    {
        InitializeComponent();
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "Up",
            UserID = "root",
            Password = "11111"
        };
        MedicalPersonnels = new List<MedicalPersonnel>();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        string login = UsernameTextBox.Text;
        string password = PasswordTextBox.Text;
        UpMedicalPersonnels();
        bool isAuthenticated = MedicalPersonnels.Any(personnel => personnel.Login == login && personnel.Password == password);
        if (isAuthenticated)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
    
    void UpMedicalPersonnels()
    {
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM MedicalPersonnel";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MedicalPersonnels.Add(new MedicalPersonnel()
                        {
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