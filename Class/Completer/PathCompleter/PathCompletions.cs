namespace Module.Completer.PathCompleter;

[AttributeUsage(
  AttributeTargets.Parameter
  | AttributeTargets.Property
  | AttributeTargets.Field
)]
public sealed class PathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
{
  public readonly string Location = string.Empty;

  public readonly PathItemType ItemType;

  public readonly bool Flat;

  public readonly bool Hidden;

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
