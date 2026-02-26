namespace Module.Tab.Path;

public partial class PathCompletionsAttribute(
  string Location = "",
  PathItemType Type = default
) : TabCompletionsAttribute<PathCompleter>
{
  public bool Flat { get; init; }

  public bool Hidden { get; init; }

  public sealed override PathCompleter Create() => new(
    Location,
    Type,
    Flat,
    Hidden,
    string.IsNullOrEmpty(
      Location
    )
  );
}
