using System.Reflection;

[assembly: AssemblyTitle("IronSmarkets.ConsoleExample")]
[assembly: AssemblyDescription("Example console .NET Client for the Smarkets exchange")]

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]
