namespace Module.Commands.Shell.Get.Directory;

[Cmdlet(
  VerbsCommon.Get,
  "Directory",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectory : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralItems",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  public string[] LiteralPath { get; set; } = [];
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectorySibling",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectorySibling : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator Location => new(
    Subpath: @".."
  );
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryRelative",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lxx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectoryRelative : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator Location => new(
    Subpath: @"..\.."
  );
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryHome",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lh")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectoryHome : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator Location => new(
    Client.Environment.Known.Folder.Home()
  );
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryCode",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lc")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectoryCode : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator Location => new(
    Client.Environment.Known.Folder.Home(),
    "code"
  );
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryDrive",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l/")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
public sealed class GetDirectoryDrive : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.Directory
  )]
  public sealed override string[] Path
  {
    get => paths;
    set => paths = value;
  }

  private protected sealed override Locator Location => new(
    CurrentDrive()
  );
}
