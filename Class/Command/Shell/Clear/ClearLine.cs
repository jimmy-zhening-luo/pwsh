namespace Module.Command.Shell.Clear;

[Cmdlet(
  VerbsCommon.Clear,
  "Line",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096807"
)]
[Alias("cl", "clear")]
[OutputType(typeof(void))]
public sealed class ClearLine : WrappedCommandShouldProcess
{
  public ClearLine() : base(
    "Clear-Content"
  )
  { }

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
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
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
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force;

  [Parameter]
  public string? Stream;

  private protected sealed override bool ValidateParameters() => !string.IsNullOrEmpty(
    path
  )
    || ParameterSetName == "LiteralPath";

  private protected sealed override void DefaultAction() => System.Console.Clear();
}
