namespace Module.Completer.PathCompleter
{
  using static System.IO.Path;

  [AttributeUsage(
    AttributeTargets.Parameter
    | AttributeTargets.Property
    | AttributeTargets.Field
  )]
  public class RelativePathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
  {
    public readonly string RelativeLocation = string.Empty;

    public readonly PathItemType ItemType;

    public readonly bool Flat;

    public RelativePathCompletionsAttribute() : base()
    { }

    public RelativePathCompletionsAttribute(
      string relativeLocation
    ) : this() => RelativeLocation = relativeLocation;

    public RelativePathCompletionsAttribute(
      string relativeLocation,
      PathItemType itemType
    ) : this(
      relativeLocation
    ) => ItemType = itemType;

    public RelativePathCompletionsAttribute(
      string relativeLocation,
      PathItemType itemType,
      bool flat
    ) : this(
      relativeLocation,
      itemType
    ) => Flat = flat;

    public override PathCompleter Create()
    {
      return new(
        GetFullPath(
          string.IsNullOrEmpty(RelativeLocation)
            ? string.Empty
            : RelativeLocation,
          PowerShell.Create(CurrentRunspace)
            .AddCommand("Get-Location")
            .Invoke()[0]
            .BaseObject
            .ToString()
            ?? string.Empty
        ),
        ItemType,
        Flat,
        string.IsNullOrEmpty(RelativeLocation)
      );
    }
  }
}
