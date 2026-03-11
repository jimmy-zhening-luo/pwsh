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
  public string Path
  { private get; init; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    init => sink = default;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public Collection<string> LiteralPath
  {
    init => sink = default;
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  {
    init => sink = default;
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  {
    init => sink = default;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => sink = default;
  }

  [Parameter]
  required public string Stream
  {
    init => sink = default;
  }

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
      _ = AddCommand(
        @"Microsoft.PowerShell.Management\Clear-Content"
      );
      _ = AddBoundParameters();

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();
    }
  }
}
