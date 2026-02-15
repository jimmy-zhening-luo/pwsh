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
  public string LiteralPath
  {
    get => literalPath;
    set => literalPath = value;
  }
  private string literalPath = "";

  [Parameter(
    ParameterSetName = "Stack"
  )]
  public string Stack
  {
    get => stack;
    set => stack = value;
  }
  private string stack = "";
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

  private protected sealed override string Location => Home();
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

  private protected sealed override string Location => Home();

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

  private protected sealed override string Location => Drive();
}

[Cmdlet(
  VerbsCommon.Set,
  "DriveD",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("d/")]
[OutputType(
  typeof(PathInfo)
)]
public sealed class SetDriveD : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "D:",
    PathItemType.Directory
  )]
  public override sealed string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => @"D:\";
}
