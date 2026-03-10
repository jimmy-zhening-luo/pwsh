namespace PowerModule.Commands.Shell.Start.Explorer;

[Cmdlet(
  VerbsLifecycle.Start,
  "Explorer",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e")]
[OutputType(typeof(void))]
sealed public class StartExplorer : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  sealed override public Collection<string> Path
  { get; init; } = [];

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public Collection<string> LiteralPath
  { private get; init; }
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerSibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("ex")]
[OutputType(typeof(void))]
sealed public class StartExplorerSibling : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions("..")]
  sealed override public Collection<string> Path
  { get; init; } = [];

  sealed override private protected string Location => Pwd("..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("exx")]
[OutputType(typeof(void))]
sealed public class StartExplorerRelative : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"..\..")]
  sealed override public Collection<string> Path
  { get; init; } = [];

  sealed override private protected string Location => Pwd(@"..\..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("eh")]
[OutputType(typeof(void))]
sealed public class StartExplorerHome : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions("~")]
  sealed override public Collection<string> Path
  { get; init; } = [];

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Home();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("ec")]
[OutputType(typeof(void))]
sealed public class StartExplorerCode : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"~\code")]
  sealed override public Collection<string> Path
  { get; init; } = [];

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Code();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerDrive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096590"
)]
[Alias("e/")]
[OutputType(typeof(void))]
sealed public class StartExplorerDrive : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"\")]
  sealed override public Collection<string> Path
  { get; init; } = [];

  sealed override private protected string Location => Drive();
}
