namespace PowerModule.Tab;

enum PathItemType
{
  Any,
  File,
  Directory,
}

sealed class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = PathItemType.Any,
 CompletionCase Case = CompletionCase.Lower
) : Factory.Intrinsics.TCompleterFactory(Case)
{
  public string Location
  { get; } = Location;

  public PathItemType ItemType
  { get; } = ItemType;

  public bool Flat
  { get; init; }

  sealed override public Completers.PathCompleter Create() => new(
    Location,
    ItemType,
    Flat,
    Case
  );
}
