using System;
using Microsoft.Win32;

namespace WindowsRegistry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Specify the Registry key and path
            string keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\YourSoftware";
            //string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\YourSoftware";

            string valueName = "YourValueName";
            string valueData = "YourValueData";


            try
            {
                // Write the value to the Registry
                Registry.SetValue(keyPath, valueName, valueData, RegistryValueKind.String);


                Console.WriteLine($"Value {valueName} successfully written to the Registry.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
