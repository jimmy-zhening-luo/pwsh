namespace Module.Tab.Path;

public partial class PathCompletionsAttribute(
  string Location,
  PathItemType ItemType,
  bool Flat
) : TabCompletionsAttribute<PathCompleter>
{
  public PathCompletionsAttribute() : this(
    string.Empty
  )
  { }

  public PathCompletionsAttribute(
    string location
  ) : this(
    location,
    PathItemType.Any
  )
  { }

  public PathCompletionsAttribute(
    string location,
    PathItemType itemType
  ) : this(
    location,
    itemType,
    false
  )
  { }

  public bool Hidden { get; init; }

  private static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = default
  )
  {
    string normalPath = Client.FileSystem.PathString.Normalize(
      path,
      preserveTrailingSeparator
    );

    string homedNormalPath = normalPath.StartsWith(
      '~'
    )
      ? normalPath.Length == 1
        ? Client.Environment.Known.Folder.Home()
        : normalPath[1] == '\\'
          ? Client.Environment.Known.Folder.Home(
              normalPath[2..]
            )
          : normalPath
        : normalPath;

    return System.IO.Path.IsPathFullyQualified(
      homedNormalPath
    )
      ? homedNormalPath
      : PowerShellHost.CurrentDirectory(
          homedNormalPath
        );
  }

  public sealed override PathCompleter Create() => new(
    Canonicalize(
      Location
    ),
    ItemType,
    Flat,
    Hidden,
    string.IsNullOrEmpty(
      Location
    )
  );
}
