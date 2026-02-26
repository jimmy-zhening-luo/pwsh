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

  public sealed override PathCompleter Create() => new(
    Canonicalizer.Canonicalize(
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
