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
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = StandardParameter.LiteralPath,
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => _ = value;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    init => _ = value;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => _ = value;
  }

  [Parameter]
  required public string Stream
  {
    init => _ = value;
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
