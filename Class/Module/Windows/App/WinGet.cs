namespace Module.Windows.App.Commands
{
  using System;
  using System.Diagnostics;
  using System.Management.Automation;

  public abstract class WinGetAppCommand : PSCoreCommand
  {
    public static string WinGet() => Context.LocalAppData() + @"\Microsoft\WindowsApps\winget.exe";

    public void CheckStatus()
    {
      var status = Var("LASTEXITCODE");

      if (
        status != null
        && (
          status.GetType() == typeof(string)
          && status != string.Empty
          && status != "0"
          && status != "1"
          || status.GetType() == typeof(int)
          && status != 0
          && status != 1
        )
      )
      {
        throw new InvalidOperationException(
          "winget.exe error, execution stopped with exit code: " + (string)status
        );
      }
    }
  }
}
