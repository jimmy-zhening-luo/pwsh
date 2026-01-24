namespace Module.Command.Shell.Get.File.Commands;

[Cmdlet(
  VerbsCommon.Get,
  "File",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p")]
[OutputType(typeof(string))]
public class GetFile : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    "",
    PathItemType.File
  )]
  public new string[]? Path;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "LiteralPath",
    Position = 1
  )]
  [SupportsWildcards]
  public new string? Filter;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileSibling",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p.", "px")]
[OutputType(typeof(string))]
public class GetFileSibling : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    "..",
    PathItemType.File
  )]
  public new string[]? Path;

  private protected override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileRelative",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p..", "pxx")]
[OutputType(typeof(string))]
public class GetFileRelative : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"..\..",
    PathItemType.File
  )]
  public new string[]? Path;

  private protected override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileHome",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("ph")]
[OutputType(typeof(string))]
public class GetFileHome : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    "~",
    PathItemType.File
  )]
  public new string[]? Path;

  private protected override string Location => Home();
}

[Cmdlet(
  VerbsCommon.Get,
  "FileCode",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pc")]
[OutputType(typeof(string))]
public class GetFileCode : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"~\code",
    PathItemType.File
  )]
  public new string[]? Path;

  private protected override string Location => Home();

  private protected override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileDrive",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p/")]
[OutputType(typeof(string))]
public class GetFileDrive : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  [PathCompletions(
    @"\",
    PathItemType.File
  )]
  public new string[]? Path;

  private protected override string Location => Drive();
}
