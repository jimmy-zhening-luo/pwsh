namespace Module.Command.Shell.Clear;

[Cmdlet(
  VerbsCommon.Clear,
  "Line",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096807"
)]
[Alias("cl", "clear")]
[OutputType(typeof(void))]
public class ClearLine : WrappedCommandShouldProcess
{
  public ClearLine() : base(
    "Clear-Content"
  )
  { }

  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions]
  public string? Path;

  [Parameter(
    Position = 1
  )]
  [SupportsWildcards]
  public string? Filter;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;

  [Parameter]
  [SupportsWildcards]
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  [Parameter]
  [Alias("f")]
  public SwitchParameter? Force;

  [Parameter]
  public string? Stream;

  private protected sealed override bool BeforeBeginProcessing() => Path != null
    || ParameterSetName == "LiteralPath";

  private protected sealed override void Default()
  {
    if (
      !IsPresent("Path")
      && !IsPresent("LiteralPath")
    )
    {
      System.Console.Clear();
    }
  }
}
