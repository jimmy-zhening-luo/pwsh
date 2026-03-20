namespace PowerModule.Commands.Shell.Move.Location;

[Cmdlet(
  VerbsCommon.Set,
  "Directory",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("c")]
[OutputType(
  typeof(PathInfo),
  typeof(PathInfoStack)
)]
sealed public class SetDirectory : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = nameof(LiteralPath),
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string LiteralPath
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = nameof(Stack)
  )]
  required public string Stack
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = nameof(C)
  )]
  public SwitchParameter C
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = nameof(D)
  )]
  public SwitchParameter D
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = nameof(E)
  )]
  public SwitchParameter E
  {
    init => _ = value;
  }

  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
  {
    [nameof(C)] = default,
    [nameof(D)] = default,
    [nameof(E)] = default,
  };

  sealed override private protected void TransformParameters()
  {
    switch (ParameterSetName)
    {
      case nameof(C):
        SetBoundParameter(
          nameof(Path),
          $"{nameof(C)}:"
        );

        break;

      case nameof(D):
        SetBoundParameter(
          nameof(Path),
          $"{nameof(D)}:"
        );

        break;

      case nameof(E):
        SetBoundParameter(
          nameof(Path),
          $"{nameof(E)}:"
        );

        break;

      default:
        break;
    }
  }
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectorySibling",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("cx")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectorySibling : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    Client.File.PathString.Parent,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryRelative",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("cxx")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryRelative : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    Client.File.PathString.ParentParent,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryHome",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("ch")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryHome : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsCommon.Set,
  "DirectoryCode",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("cc")]
[OutputType(typeof(PathInfo))]
sealed public class SetDirectoryCode : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsCommon.Set,
  "Drive",
  DefaultParameterSetName = nameof(Path),
  HelpUri = $"{HelpLink}2097049"
)]
[Alias("c/")]
[OutputType(typeof(PathInfo))]
sealed public class SetDrive : WrappedSetDirectory
{
  [Parameter(
    ParameterSetName = nameof(Path),
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    Client.File.PathString.StringSeparator,
    Tab.PathItemType.Directory
  )]
  sealed override public string Path
  { get; set; } = string.Empty;

  sealed override private protected Localizer Location => Drive;
}
