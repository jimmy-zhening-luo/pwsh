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
      string pwd = PowerShell
        .Create(RunspaceMode.CurrentRunspace)
        .AddCommand("Get-Location")
        .Invoke()[0]
        .BaseObject
        .ToString();

      return new(
        System.IO.Path.GetFullPath(
          RelativeLocation == string.Empty
            ? string.Empty
            : System.IO.Path.GetRelativePath(
                pwd,
                RelativeLocation
              ),
          pwd
        ),
        ItemType,
        Flat,
        RelativeLocation == string.Empty
      );
    }
  }
}
