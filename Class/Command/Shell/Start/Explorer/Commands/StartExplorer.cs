namespace Module.Command.Shell.Start.Explorer.Commands;

[Cmdlet(
  VerbsLifecycle.Start,
  "Explorer",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e")]
[OutputType(typeof(void))]
public sealed class StartExplorer : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions]
  public new string[]? Path;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerSibling",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e.", "ex")]
[OutputType(typeof(void))]
public sealed class StartExplorerSibling : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    ".."
  )]
  public new string[]? Path;

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerRelative",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e..", "exx")]
[OutputType(typeof(void))]
public sealed class StartExplorerRelative : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"..\.."
  )]
  public new string[]? Path;

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerHome",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("eh")]
[OutputType(typeof(void))]
public sealed class StartExplorerHome : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    "~"
  )]
  public new string[]? Path;

  private protected sealed override string Location => Home();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerCode",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("ec")]
[OutputType(typeof(void))]
public sealed class StartExplorerCode : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code"
  )]
  public new string[]? Path;

  private protected sealed override string Location => Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerDrive",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e/")]
[OutputType(typeof(void))]
public sealed class StartExplorerDrive : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"\"
  )]
  public new string[]? Path;

  private protected sealed override string Location => Drive();
}
