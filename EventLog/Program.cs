using System;
using System.Diagnostics;

internal class Program
{
    static void Main(string[] args)
    {
        string sourceName = "MyApplicationSource";

        if (!System.Diagnostics.EventLog.SourceExists(sourceName))
        {
            System.Diagnostics.EventLog.CreateEventSource(sourceName, "Application");
            Console.WriteLine($"Event source '{sourceName}' created.");
        }

        EventLog.WriteEntry(sourceName, "This is a test event log entry.", EventLogEntryType.Information);

        EventLog.WriteEntry(sourceName, "This is a test event log entry.", EventLogEntryType.Warning);

        EventLog.WriteEntry(sourceName, "This is a test event log entry.", EventLogEntryType.Error);

        Console.WriteLine("Event log entries written successfully.");
    }
}

