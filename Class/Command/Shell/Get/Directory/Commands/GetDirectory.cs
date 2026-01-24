namespace Module.Command.Shell.Get.Directory.Commands;

[Cmdlet(
  VerbsCommon.Get,
  "Directory",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectory : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.Directory
  )]
  public new string[]? Path;

  [Parameter(
    ParameterSetName = "LiteralItems",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;

  [Parameter(
    ParameterSetName = "Items",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralItems",
    Position = 1
  )]
  [SupportsWildcards]
  public new string? Filter;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectorySibling",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l.", "lx")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectorySibling : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.Directory
  )]
  public new string[]? Path;

  private protected override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryRelative",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l..", "lxx")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectoryRelative : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.Directory
  )]
  public new string[]? Path;

  private protected override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryHome",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lh")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectoryHome : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.Directory
  )]
  public new string[]? Path;

  private protected override string Location => Home();
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryCode",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lc")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectoryCode : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.Directory
  )]
  public new string[]? Path;

  private protected override string Location => Home();

  private protected override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryDrive",
  DefaultParameterSetName = "Items",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l/")]
[OutputType(
  typeof(DirectoryInfo),
  typeof(FileInfo)
)]
public class GetDirectoryDrive : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.Directory
  )]
  public new string[]? Path;

  private protected override string Location => Drive();
}
