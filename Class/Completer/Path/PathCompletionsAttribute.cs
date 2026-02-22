namespace Module.Completer.Path;

public sealed class PathCompletionsAttribute(
  string Location,
  PathItemType ItemType,
  bool Flat,
  bool Hidden
) : BaseCompletionsAttribute<PathCompleter>()
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

  public PathCompletionsAttribute(
    string location,
    PathItemType itemType,
    bool flat
  ) : this(
    location,
    itemType,
    flat,
    false
  )
  { }

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
