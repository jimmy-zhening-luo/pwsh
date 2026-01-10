namespace Module.Windows.App.Commands
{
  using System.Diagnostics;
  using System.Management.Automation;

  public abstract class WinGetAppCommand : PSCoreCommand
  {
    public static string WinGet() => Context.LocalAppData() + @"\Microsoft\WindowsApps\winget.exe";
  }
}
