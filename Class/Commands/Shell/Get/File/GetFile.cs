namespace Module.Commands.Shell.Get.File;

[Cmdlet(
  VerbsCommon.Get,
  "File",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p")]
[OutputType(typeof(string))]
public sealed class GetFile : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.File
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
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
    ParameterSetName = "Path",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralPath",
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
  "FileSibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("px")]
[OutputType(typeof(string))]
public sealed class GetFileSibling : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.File
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
  "FileRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pxx")]
[OutputType(typeof(string))]
public sealed class GetFileRelative : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.File
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
  "FileHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("ph")]
[OutputType(typeof(string))]
public sealed class GetFileHome : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.File
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
  "FileCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pc")]
[OutputType(typeof(string))]
public sealed class GetFileCode : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.File
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
  "FileDrive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p/")]
[OutputType(typeof(string))]
public sealed class GetFileDrive : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.File
  )]
  public override sealed string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override string Location => Drive();
}
