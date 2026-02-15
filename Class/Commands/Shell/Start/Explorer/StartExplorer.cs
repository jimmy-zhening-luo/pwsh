namespace Module.Commands.Shell.Start.Explorer;

[Cmdlet(
  VerbsLifecycle.Start,
  "Explorer",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e")]
[OutputType(typeof(void))]
public sealed class StartExplorer : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

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
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerSibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("ex")]
[OutputType(typeof(void))]
public sealed class StartExplorerSibling : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    ".."
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("exx")]
[OutputType(typeof(void))]
public sealed class StartExplorerRelative : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\.."
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("eh")]
[OutputType(typeof(void))]
public sealed class StartExplorerHome : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~"
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => Home();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("ec")]
[OutputType(typeof(void))]
public sealed class StartExplorerCode : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code"
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerDrive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e/")]
[OutputType(typeof(void))]
public sealed class StartExplorerDrive : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\"
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => Drive();
}
