namespace PowerModule.Commands.Shell.Start.Explorer;

[Cmdlet(
  VerbsLifecycle.Start,
  "Explorer",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("e")]
[OutputType(typeof(void))]
sealed public class StartExplorer : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  sealed override public string[] Path
  { get; init; } = [];

  [Parameter(
    ParameterSetName = StandardParameter.LiteralPath,
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => Discard();
  }
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerSibling",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("ex")]
[OutputType(typeof(void))]
sealed public class StartExplorerSibling : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.Parent)]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerRelative",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("exx")]
[OutputType(typeof(void))]
sealed public class StartExplorerRelative : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.ParentParent)]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerHome",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("eh")]
[OutputType(typeof(void))]
sealed public class StartExplorerHome : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions("~")]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerCode",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("ec")]
[OutputType(typeof(void))]
sealed public class StartExplorerCode : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"~\code")]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerDrive",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("e/")]
[OutputType(typeof(void))]
sealed public class StartExplorerDrive : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"\")]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => Drive;
}
