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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    "",
    Tab.Path.PathItemType.Directory
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
  public required string LiteralPath { get; set; }

  [Parameter(
    ParameterSetName = "Stack"
  )]
  public required string Stack { get; set; }

  [Parameter(
    ParameterSetName = "DriveC"
  )]
  public SwitchParameter C { get; set; }

  [Parameter(
    ParameterSetName = "DriveD"
  )]
  public SwitchParameter D { get; set; }

  [Parameter(
    ParameterSetName = "DriveE"
  )]
  public SwitchParameter E { get; set; }

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["C"] = default,
    ["D"] = default,
    ["E"] = default,
  };

  private protected sealed override void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "Path" when Path is "":
        Path = Pwd("..");
        break;

      case "Path":
        return;

      case "DriveC":
        Path = "C:";
        break;

      case "DriveD":
        Path = "D:";
        break;

      case "DriveE":
        Path = "E:";
        break;
    }

    BoundParameters["Path"] = Path;
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    "..",
    Tab.Path.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Subpath: @"..");
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    @"..\..",
    Tab.Path.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Subpath: @"..\..");
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    "~",
    Tab.Path.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Client.Environment.Known.Folder.Home());
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    @"~\code",
    Tab.Path.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(
    Client.Environment.Known.Folder.Code()
  );
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
    Position = default
  )]
  [SupportsWildcards]
  [Tab.Path.PathCompletions(
    Client.File.PathString.SeparatorString,
    Tab.Path.PathItemType.Directory
  )]
  public sealed override string Path
  {
    get => path;
    set => path = value;
  }

  private protected sealed override Locator Location => new(Drive());
}
