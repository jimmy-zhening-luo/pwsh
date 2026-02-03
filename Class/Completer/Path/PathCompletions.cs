namespace Module.Completer.Path;

public sealed class PathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
{
  private protected readonly string Location = string.Empty;

  private protected readonly PathItemType ItemType;

  private protected readonly bool Flat;

  private protected readonly bool Hidden;

  public PathCompletionsAttribute() : base()
  { }

  public PathCompletionsAttribute(
    string location
  ) : this() => Location = location;

  public PathCompletionsAttribute(
    string location,
    PathItemType itemType
  ) : this(
    location
  ) => ItemType = itemType;

  public PathCompletionsAttribute(
    string location,
    PathItemType itemType,
    bool flat
  ) : this(
    location,
    itemType
  ) => Flat = flat;

  public PathCompletionsAttribute(
    string location,
    PathItemType itemType,
    bool flat,
    bool hidden
  ) : this(
    location,
    itemType,
    flat
  ) => Hidden = hidden;

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
