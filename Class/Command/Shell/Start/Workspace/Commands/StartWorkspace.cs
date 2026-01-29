namespace Module.Command.Shell.Start.Workspace.Commands;

[Cmdlet(
  VerbsLifecycle.Start,
  "Workspace",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i")]
[OutputType(typeof(void))]
public sealed class StartWorkspace : VirtualStartWorkspace
{
  [PathCompletions]
  public override string Path => base.Path;
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceSibling",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ix")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceSibling : VirtualStartWorkspace
{
  [PathCompletions(
    ".."
  )]
  public override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceRelative",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ixx")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceRelative : VirtualStartWorkspace
{
  [PathCompletions(
    @"..\.."
  )]
  public override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceHome",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ih")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceHome : VirtualStartWorkspace
{
  [PathCompletions(
    "~"
  )]
  public override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Home();
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceCode",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("ic")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceCode : VirtualStartWorkspace
{
  [PathCompletions(
    @"~\code"
  )]
  public override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsLifecycle.Start,
  "WorkspaceDrive",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://code.visualstudio.com/docs/configure/command-line#_core-cli-options"
)]
[Alias("i/")]
[OutputType(typeof(void))]
public sealed class StartWorkspaceDrive : VirtualStartWorkspace
{
  [PathCompletions(
    @"\"
  )]
  public override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Drive();
}
