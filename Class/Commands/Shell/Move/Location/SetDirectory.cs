namespace PowerModule.Commands.Shell.Move.Location;

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
sealed public class SetDirectory : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; init; } = string.Empty;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string LiteralPath
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "Stack"
  )]
  required public string Stack
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "DriveC"
  )]
  public SwitchParameter C
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "DriveD"
  )]
  public SwitchParameter D
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "DriveE"
  )]
  public SwitchParameter E
  {
    init => Discard();
  }

  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
  {
    ["C"] = default,
    ["D"] = default,
    ["E"] = default,
  };

  sealed override private protected void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "DriveC":
        SetBoundParameter("Path", "C:");
        break;

      case "DriveD":
        SetBoundParameter("Path", "D:");
        break;

      case "DriveE":
        SetBoundParameter("Path", "E:");
        break;

      default:
        break;
    }
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
sealed public class SetDirectorySibling : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.Parent,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; init; } = string.Empty;

  sealed override private protected string Location => Parent();
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cxx")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryRelative : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.ParentParent,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; init; } = string.Empty;

  sealed override private protected string Location => Pwd(Client.File.PathString.ParentParent);
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("ch")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryHome : WrappedSetDirectory
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
  sealed override public string Path
  { get; init; } = string.Empty;

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Home();
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cc")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryCode : WrappedSetDirectory
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
  sealed override public string Path
  { get; init; } = string.Empty;

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Code();
}

[Cmdlet(
  VerbsCommon.Set,
  "Drive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c/")]
[OutputType(typeof(PathInfo))]
sealed public class SetDrive : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.StringSeparator,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; init; } = string.Empty;

  sealed override private protected string Location => Drive();
}
