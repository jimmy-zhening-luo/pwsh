namespace PowerModule.Commands.Shell.Clear;

[Cmdlet(
  VerbsCommon.Clear,
  "Line",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096807"
)]
[Alias("cl")]
[OutputType(typeof(void))]
sealed public class ClearLine : CoreCommand
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  public string Path { private get; set; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  required public string Filter { private get; set; }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string[] LiteralPath { private get; set; }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include { private get; set; }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude { private get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force { private get; set; }

  [Parameter]
  required public string Stream { private get; set; }

  sealed override private protected void Postprocess()
  {
    if (
      ParameterSetName is "Path"
      && Path is ""
    )
    {
      System.Console.Clear();
    }
    else
    {
      AddCommand(
        @"Microsoft.PowerShell.Management\Clear-Content"
      )
        .AddParameters(BoundParameters);

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();
    }
  }
}
