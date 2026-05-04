using System;
using System.Configuration;

namespace App_Confiq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string settingValue = ConfigurationManager.AppSettings["SettingKey"];
            Console.WriteLine($"Setting Value: {settingValue}");

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            Console.WriteLine($"Connection String: {connectionString}");
        }
    }
}
