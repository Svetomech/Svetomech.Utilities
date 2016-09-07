using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Svetomech.Utilities
{
  public static class Application
  {
    private static readonly Assembly assembly = Assembly.GetEntryAssembly();
    private static readonly FileVersionInfo assemblyInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

    public static string CompanyName => assemblyInfo.CompanyName;
    public static string ProductName => assemblyInfo.ProductName;
    public static string ProductVersion => assemblyInfo.ProductVersion;
    public static string ExecutablePath => assembly.Location;
    public static string StartupPath => Path.GetDirectoryName(assembly.Location);
    public static string AssemblyGuid => ((GuidAttribute)assembly.GetCustomAttributes(
      typeof(GuidAttribute), false).GetValue(0)).Value;
  }
}
