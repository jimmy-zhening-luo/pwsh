namespace Module.Commands.Shell.Start.Workspace;

[Cmdlet(
  VerbsLifecycle.Start,
  "Workspace",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i")]
[OutputType(typeof(void))]
public sealed class StartWorkspace : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceSibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ix")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceSibling : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions("..")]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Subpath: @"..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ixx")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceRelative : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions(@"..\..")]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Subpath: @"..\..");
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ih")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceHome : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions("~")]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Client.Environment.Known.Folder.Home());
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ic")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceCode : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions(@"~\code")]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(
    Client.Environment.Known.Folder.Home(),
    "code"
  );
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceDrive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i/")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceDrive : VirtualStartWorkspace
{
  [Parameter(
    Position = default
  )]
  [PathCompletions(@"\")]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Drive());
}
