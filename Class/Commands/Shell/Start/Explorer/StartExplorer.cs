namespace Module.Commands.Shell.Start.Explorer;

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
  sealed override public string[] Path { get; set; } = [];

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string[] LiteralPath { private get; set; }
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
  sealed override public string[] Path { get; set; } = [];

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
  sealed override public string[] Path { get; set; } = [];

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
  sealed override public string[] Path { get; set; } = [];

  sealed override private protected string Location { get; } = Client.Environment.Known.Folder.Home();
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
  sealed override public string[] Path { get; set; } = [];

  sealed override private protected string Location { get; } = Client.Environment.Known.Folder.Code();
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
  sealed override public string[] Path { get; set; } = [];

  sealed override private protected string Location => Drive();
}
