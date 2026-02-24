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
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions]
  public string Path { get; set; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  public string Filter { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath { get; set; } = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Include { get; set; } = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude { get; set; } = [];

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  [Parameter]
  public string Stream { get; set; } = string.Empty;

  private protected sealed override void AfterEndProcessing()
  {
    if (
      ParameterSetName == "Path"
      && string.IsNullOrEmpty(
        Path
      )
    )
    {
      System.Console.Clear();
    }
    else
    {
      AddCommand(
        "Clear-Content"
      )
        .AddParameters(
          MyInvocation.BoundParameters
        );

      using var steppablePipeline = PS.GetSteppablePipeline();

      steppablePipeline.Begin(
        this
      );

      steppablePipeline.Process();

      steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();
    }
  }
}
