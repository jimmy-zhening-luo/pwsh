namespace Completer.PathCompleter
{
  using System;
  using System.Management.Automation;

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

    public RelativePathCompletionsAttribute() : base() { }

    public RelativePathCompletionsAttribute(string relativeLocation) : this() => RelativeLocation = relativeLocation;

    public RelativePathCompletionsAttribute(
      string relativeLocation,
      PathItemType itemType
    ) : this(relativeLocation) => ItemType = itemType;

    public RelativePathCompletionsAttribute(
      string relativeLocation,
      PathItemType itemType,
      bool flat
    ) : this(relativeLocation, itemType) => Flat = flat;

    public override PathCompleter Create()
    {
      return new(
        System.IO.Path.GetFullPath(
          string.IsNullOrEmpty(RelativeLocation)
            ? string.Empty
            : RelativeLocation,
          PowerShell
            .Create(RunspaceMode.CurrentRunspace)
            .AddCommand("Get-Location")
            .Invoke()[0]
            .BaseObject
            .ToString()
        ),
        ItemType,
        Flat,
        string.IsNullOrEmpty(RelativeLocation)
      );
    }
  }
}
