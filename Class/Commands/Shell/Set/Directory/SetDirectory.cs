namespace Module.Commands.Shell.Set.Directory;

[Cmdlet(
  VerbsCommon.Set,
  "Directory",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c")]
[OutputType(
  typeof(PathInfo),
  typeof(PathInfoStack)
)]
public sealed class SetDirectory : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string LiteralPath { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "Stack"
  )]
  public string Stack { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "DriveC"
  )]
  public SwitchParameter C
  {
    get => driveC;
    set => driveC = value;
  }
  private bool driveC;

  [Parameter(
    ParameterSetName = "DriveD"
  )]
  public SwitchParameter D
  {
    get => driveD;
    set => driveD = value;
  }
  private bool driveD;

  [Parameter(
    ParameterSetName = "DriveE"
  )]
  public SwitchParameter E
  {
    get => driveE;
    set => driveE = value;
  }
  private bool driveE;

  private protected sealed override void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "Path":
        if (path is not "")
        {
          return;
        }

        path = Pwd(
          ".."
        );

        break;
      case "DriveC":
        driveC = false;
        BoundParameters.Remove("C");
        path = "C:";

        break;
      case "DriveD":
        driveD = false;
        BoundParameters.Remove("D");
        path = "D:";

        break;
      case "DriveE":
        driveE = false;
        BoundParameters.Remove("E");
        path = "E:";

        break;
      default:
        return;
    }

    BoundParameters["Path"] = path;
  }
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectorySibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cx")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDirectorySibling : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cxx")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDirectoryRelative : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("ch")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDirectoryHome : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Client.Environment.Known.Folder.Home();
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cc")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDirectoryCode : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Client.Environment.Known.Folder.Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Set,
  "Drive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c/")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDrive : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => CurrentDrive();
}
