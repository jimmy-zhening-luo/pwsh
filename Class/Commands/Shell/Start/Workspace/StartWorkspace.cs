namespace PowerModule.Commands.Shell.Start.Workspace;

[Cmdlet(
  VerbsLifecycle.Start,
  "Workspace",
  DefaultParameterSetName = nameof(Path)
)]
[Alias("i")]
[OutputType(typeof(void))]
sealed public class StartWorkspace : VirtualStartWorkspace
{
  [Parameter(Position = default)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  sealed override public string Path
  { private protected get; init; } = string.Empty;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceSibling",
  DefaultParameterSetName = nameof(Path)
)]
[Alias("ix")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceSibling : VirtualStartWorkspace
{
  [Parameter(Position = default)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(Client.File.PathString.Parent)]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceRelative",
  DefaultParameterSetName = nameof(Path)
)]
[Alias("ixx")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceRelative : VirtualStartWorkspace
{
  [Parameter(Position = default)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(Client.File.PathString.ParentParent)]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceHome",
  DefaultParameterSetName = nameof(Path)
)]
[Alias("ih")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceHome : VirtualStartWorkspace
{
  [Parameter(Position = default)]
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
  DefaultParameterSetName = nameof(Path)
)]
[Alias("ic")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceCode : VirtualStartWorkspace
{
  [Parameter(Position = default)]
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
  DefaultParameterSetName = nameof(Path)
)]
[Alias("i/")]
[OutputType(typeof(void))]
sealed public class StartWorkspaceDrive : VirtualStartWorkspace
{
  [Parameter(Position = default)]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(@"\")]
  sealed override public string Path
  { private protected get; init; } = string.Empty;

  sealed override private protected Localizer Location => Drive;
}
