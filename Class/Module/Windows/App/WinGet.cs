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
        status is int istatus
        && istatus != 0
        && istatus != 1
      )
      {
        throw new InvalidOperationException(
          "winget.exe error, execution stopped with exit code: " + istatus.ToString()
        );
      }
    }
  }

  [Cmdlet(
    VerbsCommon.Add,
    "WinGetApp",
    HelpUri = "https://learn.microsoft.com/en-us/windows/package-manager/winget/install"
  )]
  [Alias("wga")]
  public class AddWinGetApp : WinGetAppCommand
  {
    [Parameter(
      Position = 0,
      ValueFromRemainingArguments = true,
      HelpMessage = "Arguments for winget install"
    )]
    [AllowEmptyCollection]
    public string[] Argument
    {
      get => arguments;
      set => arguments = value;
    }
    private string[] arguments = [];

    protected override void EndProcessing()
    {
      if (arguments.Length == 0)
      {
        Call(
          WinGet(),
          "upgrade"
        );
      }
      else
      {
        Call(
          WinGet(),
          "install",
          arguments
        );
      }

      CheckStatus();
    }
  }
}
