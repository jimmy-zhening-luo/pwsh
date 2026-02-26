namespace Module.Commands.Shell.Clear;

[Cmdlet(
  VerbsCommon.Clear,
  "Line",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096807"
)]
[Alias("cl", "clear")]
[OutputType(typeof(void))]
public sealed class ClearLine : CoreCommand
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions]
  public string Path { get; set; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  public required string Filter { get; set; }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public required string[] LiteralPath { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Include { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force { get; set; }

  [Parameter]
  public required string Stream { get; set; }

  private protected sealed override void Postprocess()
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
        "Clear-Content"
      )
        .AddParameters(
          BoundParameters
        );

      using var steppablePipeline = PS.GetSteppablePipeline();

      steppablePipeline.Begin(
        this
      );

      _ = steppablePipeline.Process();

      _ = steppablePipeline.End();

      steppablePipeline.Clean();
      steppablePipeline.Dispose();
    }
  }
}
