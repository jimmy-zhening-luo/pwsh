namespace PowerModule.Commands.Shell.Read.File;

[Cmdlet(
  VerbsCommon.Get,
  "File",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p")]
[OutputType(typeof(string))]
sealed public class GetFile : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public Collection<string> LiteralPath
  { private get; init; }
}

[Cmdlet(
  VerbsCommon.Get,
  "FileSibling",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("px")]
[OutputType(typeof(string))]
sealed public class GetFileSibling : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "..",
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  sealed override private protected string Location => Pwd("..");
}

[Cmdlet(
  VerbsCommon.Get,
  "FileRelative",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pxx")]
[OutputType(typeof(string))]
sealed public class GetFileRelative : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"..\..",
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  sealed override private protected string Location => Pwd(@"..\..");
}

[Cmdlet(
  VerbsCommon.Get,
  "FileHome",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("ph")]
[OutputType(typeof(string))]
sealed public class GetFileHome : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Home();
}

[Cmdlet(
  VerbsCommon.Get,
  "FileCode",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pc")]
[OutputType(typeof(string))]
sealed public class GetFileCode : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  sealed override private protected string Location
  { get; } = Client.Environment.Folder.Code();
}

[Cmdlet(
  VerbsCommon.Get,
  "FileDrive",
  DefaultParameterSetName = "Path",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p/")]
[OutputType(typeof(string))]
sealed public class GetFileDrive : WrappedGetFile
{
  [Parameter(
    ParameterSetName = "Path",
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.SeparatorString,
    Tab.PathItemType.File
  )]
  sealed override public Collection<string> Path
  {
    init => paths = value;
  }

  sealed override private protected string Location => Drive();
}
