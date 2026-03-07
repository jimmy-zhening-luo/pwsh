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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public required string[] LiteralPath
  {
    private get;
    set;
  }
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions("..")]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  private protected sealed override string Location => Pwd("..");
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"..\..")]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  private protected sealed override string Location => Pwd(@"..\..");
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions("~")]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  private protected sealed override string Location { get; } = Client.Environment.Known.Folder.Home();
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"~\code")]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  private protected sealed override string Location { get; } = Client.Environment.Known.Folder.Code();
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"\")]
  public sealed override string[] Path
  {
    set => paths = value;
  }

  private protected sealed override string Location => Drive();
}
