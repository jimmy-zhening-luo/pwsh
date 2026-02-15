namespace Module.Commands.Shell.Get.Directory;

[Cmdlet(
  VerbsCommon.Get,
  "Directory",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectory : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralItems",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath
  {
    get => literalPaths;
    set => literalPaths = value;
  }
  private string[] literalPaths = [];

  [Parameter(
    ParameterSetName = "Items",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralItems",
    Position = 1
  )]
  [SupportsWildcards]
  public override sealed string Filter
  {
    get => filter;
    set => filter = value;
  }
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectorySibling",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lx")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectorySibling : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryRelative",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lxx")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectoryRelative : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryHome",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lh")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectoryHome : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => PC.Environment.Known.Folder.Home();
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryCode",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lc")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectoryCode : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => PC.Environment.Known.Folder.Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryDrive",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l/")]
[OutputType(
  typeof(IO.DirectoryInfo),
  typeof(IO.FileInfo)
)]
public sealed class GetDirectoryDrive : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.Directory
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => Drive();
}
