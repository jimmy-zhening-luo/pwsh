namespace Module.Command.Shell.Set.Directory.Commands;

[Cmdlet(
  VerbsCommon.Set,
  "Directory",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c")]
[OutputType(
  typeof(PathInfo),
  typeof(PathInfoStack)
)]
public class SetDirectory : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.Directory
  )]
  public new string? Path;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string? LiteralPath;

  [Parameter(
    ParameterSetName = "Stack",
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  public string? Stack;
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectorySibling",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c.", "cx")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDirectorySibling : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryRelative",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c..", "cxx")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDirectoryRelative : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryHome",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("ch")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDirectoryHome : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string Location => Home();
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryCode",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("cc")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDirectoryCode : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string Location => Home();

  private protected override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Set,
  "Drive",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("c/")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDrive : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string Location => Drive();
}

[Cmdlet(
  VerbsCommon.Set,
  "DriveD",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
)]
[Alias("d/")]
[OutputType(
  typeof(PathInfo)
)]
public class SetDriveD : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyString]
  [SupportsWildcards]
  [PathCompletions(
    "D:",
    PathItemType.Directory
  )]
  public new string? Path;

  private protected override string Location => @"D:\";
}
