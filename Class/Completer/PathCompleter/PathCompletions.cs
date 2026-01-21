namespace Module.Completer.PathCompleter;

[AttributeUsage(
  AttributeTargets.Parameter
  | AttributeTargets.Property
  | AttributeTargets.Field
)]
public class PathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
{
  public readonly string Location = string.Empty;

  public readonly PathItemType ItemType;

  public readonly bool Flat;

  public PathCompletionsAttribute() : base()
  { }

  public PathCompletionsAttribute(
    string location
  ) : this() => Location = AnchorHome(
    Normalize(
      location
    )
  );

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

  public override PathCompleter Create()
  {
    bool nullPath = string.IsNullOrEmpty(
      Location
    );

    return new(
      Path.GetFullPath(
        nullPath
          ? string.Empty
          : Location,
        PS()
          .AddCommand("Get-Location")
          .Invoke()[0]
          .BaseObject
          .ToString()
          ?? string.Empty
      ),
      ItemType,
      Flat,
      nullPath
    );
  }
}
