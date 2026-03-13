namespace PowerModule.Commands.Shell.Start.Workspace;

[Cmdlet(
  VerbsLifecycle.Start,
  "Workspace",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i")]
[OutputType(typeof(void))]
sealed public class StartWorkspace : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  sealed override public string Path
  { private protected get; init; } = string.Empty;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceSibling",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ix")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceSibling : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(Client.File.PathString.Parent)]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceRelative",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ixx")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceRelative : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(Client.File.PathString.ParentParent)]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceHome",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ih")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceHome : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions("~")]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceCode",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ic")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceCode : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(@"~\code")]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceDrive",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i/")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceDrive : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(@"\")]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => Drive;
}
