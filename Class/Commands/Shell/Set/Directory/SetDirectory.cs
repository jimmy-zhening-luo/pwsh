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
  sealed override public string Path { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string LiteralPath { private get; set; }

  [Parameter(
    ParameterSetName = "Stack"
  )]
  required public string Stack { private get; set; }

  [Parameter(
    ParameterSetName = "DriveC"
  )]
  public SwitchParameter C { private get; set; }

  [Parameter(
    ParameterSetName = "DriveD"
  )]
  public SwitchParameter D { private get; set; }

  [Parameter(
    ParameterSetName = "DriveE"
  )]
  public SwitchParameter E { private get; set; }

  sealed override private protected Dictionary<string, object?> CoercedParameters { get; } = new()
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
        BoundParameters["Path"] = Path = "C:";
        break;

      case "DriveD":
        BoundParameters["Path"] = Path = "D:";
        break;

      case "DriveE":
        BoundParameters["Path"] = Path = "E:";
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
    "..",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path { get; set; } = string.Empty;

  sealed override private protected string Location => Pwd("..");
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
    @"..\..",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path { get; set; } = string.Empty;

  sealed override private protected string Location => Pwd(@"..\..");
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
  sealed override public string Path { get; set; } = string.Empty;

  sealed override private protected string Location { get; } = Client.Environment.Known.Folder.Home();
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
  sealed override public string Path { get; set; } = string.Empty;

  sealed override private protected string Location { get; } = Client.Environment.Known.Folder.Code();
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
    Client.File.PathString.SeparatorString,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path { get; set; } = string.Empty;

  sealed override private protected string Location => Drive();
}
