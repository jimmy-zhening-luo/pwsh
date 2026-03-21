namespace PowerModule.Commands.Shell.Start.Explorer;

[Cmdlet(
  VerbsLifecycle.Start,
  "Explorer",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("e")]
[OutputType(typeof(void))]
sealed public class StartExplorer : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions]
  sealed override public string[] Path
  { get; set; } = [];

  [Parameter(
    ParameterSetName = nameof(LiteralPath),
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => _ = value;
  }
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerSibling",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("ex")]
[OutputType(typeof(void))]
sealed public class StartExplorerSibling : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.Parent)]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerRelative",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("exx")]
[OutputType(typeof(void))]
sealed public class StartExplorerRelative : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.ParentParent)]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerHome",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("eh")]
[OutputType(typeof(void))]
sealed public class StartExplorerHome : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.StringHome)]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerCode",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("ec")]
[OutputType(typeof(void))]
sealed public class StartExplorerCode : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(@"~\code")]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "ExplorerDrive",
  DefaultParameterSetName = nameof(Path),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096590"
)]
[Alias("e/")]
[OutputType(typeof(void))]
sealed public class StartExplorerDrive : WrappedStartExplorer
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(Client.File.PathString.StringSeparator)]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => Drive;
}
