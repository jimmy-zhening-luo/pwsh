namespace PowerModule.Commands.Shell.Read.Directory;

[Cmdlet(
  VerbsCommon.Get,
  "Directory",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("l")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectory : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  [Parameter(
    ParameterSetName = "LiteralItems",
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => _ = value;
  }
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectorySibling",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("lx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectorySibling : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.Parent,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryRelative",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("lxx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryRelative : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.ParentParent,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryHome",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("lh")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryHome : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryCode",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("lc")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryCode : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryDrive",
  DefaultParameterSetName = DefaultParameterSet,
  HelpUri = $"{HelpLink}2096492"
)]
[Alias("l/")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryDrive : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = DefaultParameterSet,
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.StringSeparator,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; set; } = [];

  sealed override private protected Localizer Location => Drive;
}
