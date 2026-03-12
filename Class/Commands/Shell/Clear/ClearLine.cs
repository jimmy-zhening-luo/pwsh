namespace PowerModule.Commands.Shell.Clear;

[Cmdlet(
  VerbsCommon.Clear,
  "Line",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096807"
)]
[Alias("cl")]
[OutputType(typeof(void))]
sealed public class ClearLine : CoreCommand
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
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
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string[] LiteralPath
  {
    init => Discard();
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    init => Discard();
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => Discard();
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => Discard();
  }

  [Parameter]
  required public string Stream
  {
    init => Discard();
  }

  sealed override private protected void Postprocess()
  {
    if (
      ParameterSetName is StandardParameter.Path
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
