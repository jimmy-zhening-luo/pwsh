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
  public string Path
  {
    get => path;
    set => path = value;
  }
  private string path = "";

  [Parameter(
    Position = 1
  )]
  [SupportsWildcards]
  public string Filter
  {
    get => filter;
    set => filter = value;
  }
  private string filter = "";

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath
  {
    get => literalPaths;
    set => literalPaths = value;
  }
  private string[] literalPaths = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Include
  {
    get => inclusions;
    set => inclusions = value;
  }
  private string[] inclusions = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude
  {
    get => exclusions;
    set => exclusions = value;
  }
  private string[] exclusions = [];

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    get => force;
    set => force = value;
  }
  private bool force;

  [Parameter]
  public string Stream
  {
    get => stream;
    set => stream = value;
  }
  private string stream = "";

  private protected sealed override void AfterEndProcessing()
  {
    if (
      ParameterSetName == "Path"
      && string.IsNullOrEmpty(
        path
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
