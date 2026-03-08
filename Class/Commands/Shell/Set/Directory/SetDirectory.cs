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
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public required string LiteralPath
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "Stack"
  )]
  public required string Stack
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "DriveC"
  )]
  public SwitchParameter C
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "DriveD"
  )]
  public SwitchParameter D
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "DriveE"
  )]
  public SwitchParameter E
  {
    private get;
    set;
  }

  private protected sealed override Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["C"] = default,
    ["D"] = default,
    ["E"] = default,
  };

  private protected sealed override void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "Path" when path is "":
        path = Pwd("..");
        break;

      case "Path":
        return;

      case "DriveC":
        path = "C:";
        break;

      case "DriveD":
        path = "D:";
        break;

      case "DriveE":
        path = "E:";
        break;
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
[OutputType(typeof(PathInfo))]
public sealed class SetDirectorySibling : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "..",
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Pwd("..");
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cxx")]
[OutputType(typeof(PathInfo))]
public sealed class SetDirectoryRelative : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"..\..",
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Pwd(@"..\..");
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("ch")]
[OutputType(typeof(PathInfo))]
public sealed class SetDirectoryHome : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location { get; } = Client.Environment.Known.Folder.Home();
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cc")]
[OutputType(typeof(PathInfo))]
public sealed class SetDirectoryCode : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location { get; } = Client.Environment.Known.Folder.Code();
}

[Cmdlet(
  VerbsCommon.Set,
  "Drive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c/")]
[OutputType(typeof(PathInfo))]
public sealed class SetDrive : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.SeparatorString,
    Tab.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override string Location => Drive();
}
