namespace PowerModule.Tab;

sealed class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = PathItemType.Any,
  CompletionCase Case = CompletionCase.Lower
) : Factory.Intrinsics.TCompleterFactory
{
  sealed override public CompletionCase Case
  { get; init; } = Case;

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
