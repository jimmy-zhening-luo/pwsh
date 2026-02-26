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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath { get; set; } = [];
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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator LocationRecord => new(
    Subpath: @".."
  );
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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator LocationRecord => new(
    Subpath: @"..\.."
  );
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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator LocationRecord => new(
    Client.Environment.Known.Folder.Home()
  );
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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator LocationRecord => new(
    Client.Environment.Known.Folder.Home(),
    "code"
  );
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
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.File
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator LocationRecord => new(
    CurrentDrive()
  );
}
