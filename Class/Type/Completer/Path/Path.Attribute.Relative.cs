using System;
using System.Management.Automation;

namespace Completer
{
  namespace PathCompleter
  {
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RelativePathCompletionsAttribute : PathCompletionsAttribute
    {
      public RelativePathCompletionsAttribute(ScriptBlock currentDirectory) : this(
        currentDirectory,
        PathItemType.Any,
        false
      )
      { }

      public RelativePathCompletionsAttribute(
        ScriptBlock currentDirectory,
        PathItemType itemType
      ) : this(
        currentDirectory,
        itemType,
        false
      )
      { }

      public RelativePathCompletionsAttribute(
        ScriptBlock currentDirectory,
        PathItemType itemType,
        bool flat
      ) : base(
        currentDirectory
          .Invoke()[0]
          .BaseObject
          .ToString(),
        itemType,
        flat
      )
      { }
    }
  } // namespace PathCompleter
} // namespace Completer
