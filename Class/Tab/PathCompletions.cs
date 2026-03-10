namespace PowerModule.Tab;

internal enum PathItemType
{
  Any,
  File,
  Directory,
}

sealed internal class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = PathItemType.Any,
 CompletionCase Case = CompletionCase.Lower
) : Factory.TCompletionsAttribute(Case)
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
