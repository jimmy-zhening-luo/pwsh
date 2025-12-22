using System.IO;
using System.Collections.Generic;

namespace Completer
{
  namespace PathCompleter
  {
    public class PathCompleter : BaseCompleter
    {
      public readonly string Root;
      public readonly PathItemType Type;
      public readonly bool Flat;

      private PathCompleter() : base() { }

      public PathCompleter(
        string root,
        PathItemType type,
        bool flat
      ) : this()
      {
        Root = Resolver.AnchorHome(
          Canonicalizer.Normalize(root)
        );
        Type = type;
        Flat = flat;
      }

      public override IEnumerable<string> FulfillCompletion(string wordToComplete)
      {
        string currentPathValue = Canonicalizer.Normalize(
          wordToComplete,
          true
        );
        string currentDirectoryValue = "";
        string searchLocation = "";
        string searchFilter = "";

        if (!string.IsNullOrWhiteSpace(currentPathValue))
        {
          int lastSeparatorIndex = currentPathValue.LastIndexOf(
            '\\'
          );

          if (lastSeparatorIndex >= 0)
          {
            string beforeSeparator = currentPathValue[..lastSeparatorIndex];
            string afterSeparator = lastSeparatorIndex + 1 < currentPathValue.Length
              ? currentPathValue[(lastSeparatorIndex + 1)..]
              : string.Empty;

            if (!string.IsNullOrWhiteSpace(beforeSeparator))
            {
              currentDirectoryValue = beforeSeparator;
            }

            if (!string.IsNullOrWhiteSpace(afterSeparator))
            {
              searchFilter = afterSeparator;
            }
          }
          else
          {
            searchFilter = currentPathValue;
          }

          if (currentDirectoryValue != string.Empty)
          {
            string fullPathAndCurrentDirectory = Path.GetFullPath(
              currentDirectoryValue,
              Root
            );

            if (
              Directory.Exists(
                fullPathAndCurrentDirectory
              )
            )
            {
              searchLocation = fullPathAndCurrentDirectory;

            }
          }
        }

        if (searchLocation == string.Empty)
        {
          searchLocation = Root;
        }

        searchFilter = searchFilter + "*";
        EnumerationOptions attributes = new EnumerationOptions();

        attributes.IgnoreInaccessible = false;

        if (Type != PathItemType.File)
        {
          foreach (
            string directory in Directory.EnumerateDirectories(
              searchLocation,
              searchFilter,
              attributes
            )
          )
          {
            yield return Canonicalizer.Denormalize(
              currentDirectoryValue,
              Path.GetFileName(directory),
              Flat
                ? string.Empty
                : @"\"
            );
          }
        }

        if (Type != PathItemType.Directory)
        {
          foreach (
            string file in Directory.EnumerateFiles(
              searchLocation,
              searchFilter,
              attributes
            )
          )
          {
            yield return Canonicalizer.Denormalize(
              currentDirectoryValue,
              Path.GetFileName(file)
            );
          }
        }

        yield return Canonicalizer.Denormalize(
          currentDirectoryValue,
          @"..\"
        );

        if (currentDirectoryValue != string.Empty)
        {
          yield return Canonicalizer.Denormalize(
            currentDirectoryValue,
            @"\"
          );
        }

        yield break;
      }
    }
  } // namespace PathCompleter
} // namespace Completer
