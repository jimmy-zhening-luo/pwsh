namespace PowerModule.Commands.Shell.Start.Workspace;

[Cmdlet(
  VerbsLifecycle.Start,
  "Workspace",
  DefaultParameterSetName = "Path",
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
  { private protected get; set; } = string.Empty;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceSibling",
  DefaultParameterSetName = "Path",
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
  [Tab.PathCompletions("..")]
  sealed override public string Path
  { private protected get; set; } = string.Empty;

  sealed override private protected string Location => Pwd("..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceRelative",
  DefaultParameterSetName = "Path",
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
  [Tab.PathCompletions(@"..\..")]
  sealed override public string Path
  { private protected get; set; } = string.Empty;

  sealed override private protected string Location => Pwd(@"..\..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceHome",
  DefaultParameterSetName = "Path",
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
  { private protected get; set; } = string.Empty;

  sealed override private protected string Location
  { get; } = Client.Environment.Known.Folder.Home();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceCode",
  DefaultParameterSetName = "Path",
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
  { private protected get; set; } = string.Empty;

  sealed override private protected string Location
  { get; } = Client.Environment.Known.Folder.Code();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceDrive",
  DefaultParameterSetName = "Path",
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
  { private protected get; set; } = string.Empty;

  sealed override private protected string Location => Drive();
}
