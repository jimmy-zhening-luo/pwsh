namespace PowerModule.Tab;

sealed class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = default
) : Factory.Intrinsics.CompleterFactory
{
  sealed override public CompletionCase Case
  { get; init; } = CompletionCase.Lower;

  public string Location
  { get; } = Location;

  public PathItemType ItemType
  { get; } = ItemType;

  public bool Flat
  { get; init; }

  sealed override public Completers.PathCompleter Create() => new(
    Case,
    Location,
    ItemType,
    Flat
  );
}
