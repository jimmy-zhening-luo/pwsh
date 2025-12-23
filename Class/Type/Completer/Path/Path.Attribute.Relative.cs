using System;
using System.Management.Automation;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RelativePathCompletionsAttribute : BaseCompletionsAttribute<PathCompleter>
    {
      public readonly ScriptBlock CurrentDirectory;
      public readonly PathItemType ItemType;
      public readonly bool Flat;

      private RelativePathCompletionsAttribute() : base() { }

      public RelativePathCompletionsAttribute(ScriptBlock currentDirectory) : this() => CurrentDirectory = currentDirectory;

      public RelativePathCompletionsAttribute(
        ScriptBlock currentDirectory,
        PathItemType itemType
      ) : this(currentDirectory) => ItemType = itemType;

      public RelativePathCompletionsAttribute(
        ScriptBlock currentDirectory,
        PathItemType itemType,
        bool flat
      ) : this(currentDirectory, itemType) => Flat = flat;

      public override PathCompleter Create() => new(
        CurrentDirectory
          .Invoke()[0]
          .BaseObject
          .ToString(),
        ItemType,
        Flat
      );
    }
  } // namespace PathCompleter
} // namespace Completer
