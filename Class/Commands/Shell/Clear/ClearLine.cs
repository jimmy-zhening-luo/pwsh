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
  { private get; init; }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public Collection<string> LiteralPath
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  { private get; init; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  { private get; init; }

  [Parameter]
  required public string Stream
  { private get; init; }

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
