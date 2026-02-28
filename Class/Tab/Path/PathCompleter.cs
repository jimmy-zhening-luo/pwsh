namespace Module.Tab.Path;

public sealed class PathCompleter : TabCompleter
{
  private readonly string Location;

  private readonly PathItemType ItemType;

  private readonly bool Flat;

  private readonly bool IncludeHidden;

  private readonly bool AllowReanchor;

  private uint Index;

  public PathCompleter(
    string location,
    PathItemType itemType,
    bool flat,
    bool hidden
  ) => (
    Location,
    ItemType,
    Flat,
    IncludeHidden,
    AllowReanchor
  ) = (
    Client.File.PathString.Normalize(location) is var normalPath
    && System.IO.Path.IsPathFullyQualified(normalPath)
      ? normalPath
      : PowerShellHost.FullPathCurrentLocationRelative(normalPath),
    itemType,
    flat,
    hidden,
    location is ""
  );

  private static (string, string, string) ParsePathToComplete(
    string wordToComplete,
    string location,
    bool allowReanchor
  )
  {
    var pathToComplete = Client.File.PathString.Normalize(
      wordToComplete,
      true
    );
    var accumulatedPath = string.Empty;
    var accumulatedPathSubpathPart = string.Empty;
    var leaf = string.Empty;

    while (pathToComplete is not "")
    {
      var pathEnd = pathToComplete.LastIndexOf('\\');

      if (pathEnd < 0)
      {
        leaf = pathToComplete;
        pathToComplete = string.Empty;
      }
      else
      {
        var subpathPart = pathToComplete[..pathEnd].Trim();
        var wordStart = pathEnd + 1;

        if (wordStart < pathToComplete.Length)
        {
          leaf = pathToComplete[wordStart..].Trim();
        }

        if (subpathPart is "")
        {
          pathToComplete = string.Empty;
        }
        else
        {
          var anchoredPath = System.IO.Path.GetFullPath(
            subpathPart,
            location
          );

          if (
            System.IO.Directory.Exists(
              anchoredPath
            )
          )
          {
            accumulatedPathSubpathPart = System.IO.Path.GetRelativePath(
              location,
              anchoredPath
            );
            accumulatedPath = anchoredPath;
            pathToComplete = string.Empty;
          }
          else if (
            allowReanchor
            && System.IO.Directory.Exists(subpathPart)
          )
          {
            accumulatedPathSubpathPart = System.IO.Path.GetFullPath(subpathPart);
            accumulatedPath = accumulatedPathSubpathPart;
            pathToComplete = string.Empty;
          }
          else
          {
            leaf = string.Empty;
            pathToComplete = subpathPart;
          }
        }
      }
    }

    if (accumulatedPath is "")
    {
      accumulatedPath = location;
    }

    return (
      accumulatedPath,
      accumulatedPathSubpathPart,
      leaf + "*"
    );
  }

  private static string JoinPathCompletion(
    string filename,
    string accumulatedSubpath,
    bool trailingSeparator = default
  ) => System.IO.Path.Join(
    accumulatedSubpath,
    filename,
    trailingSeparator
      ? @"\"
      : string.Empty
  )
    .Replace('\\', '/');

  private protected sealed override IEnumerable<string> GenerateCompletion(string wordToComplete)
  {
    Index = default;

    var (
      searchPath,
      accumulator,
      searchFilter
    ) = ParsePathToComplete(
      wordToComplete,
      Location,
      AllowReanchor
    );

    System.IO.EnumerationOptions searchOptions = new()
    {
      IgnoreInaccessible = default
    };
    if (IncludeHidden)
    {
      searchOptions.AttributesToSkip = System.IO.FileAttributes.System;
    }
    var originalAttributes = searchOptions.AttributesToSkip;

    switch (ItemType)
    {
      case PathItemType.Directory:
        foreach (
          var directory in EnumerateDirectories(
            searchPath,
            searchFilter,
            searchOptions,
            accumulator,
            !Flat
          )
        )
        {
          yield return directory;
        }

        if (
          Index is 0
          && searchFilter.Length > 1
          && searchOptions is not
          {
            AttributesToSkip: System.IO.FileAttributes.System
          }
        )
        {
          searchOptions.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchPath,
              searchFilter,
              searchOptions,
              accumulator,
              !Flat
            )
          )
          {
            yield return directory;
          }

          searchOptions.AttributesToSkip = originalAttributes;
        }

        break;

      case PathItemType.File:
        foreach (
          var file in EnumerateFiles(
            searchPath,
            searchFilter,
            searchOptions,
            accumulator
          )
        )
        {
          yield return file;
        }

        if (
          Index is 0
          && searchFilter.Length > 1
          && searchOptions is not
          {
            AttributesToSkip: System.IO.FileAttributes.System
          }
        )
        {
          searchOptions.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var file in EnumerateFiles(
              searchPath,
              searchFilter,
              searchOptions,
              accumulator
            )
          )
          {
            yield return file;
          }

          searchOptions.AttributesToSkip = originalAttributes;
        }

        var checkpoint = Index;

        foreach (
          var directory in EnumerateDirectories(
            searchPath,
            searchFilter,
            searchOptions,
            accumulator,
            true
          )
        )
        {
          yield return directory;
        }

        if (
          Index == checkpoint
          && searchFilter.Length > 1
          && searchOptions is not
          {
            AttributesToSkip: System.IO.FileAttributes.System
          }
        )
        {
          searchOptions.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchPath,
              searchFilter,
              searchOptions,
              accumulator,
              true
            )
          )
          {
            yield return directory;
          }

          searchOptions.AttributesToSkip = originalAttributes;
        }

        break;

      default:
        foreach (
          var directory in EnumerateDirectories(
            searchPath,
            searchFilter,
            searchOptions,
            accumulator,
            !Flat
          )
        )
        {
          yield return directory;
        }

        foreach (
          var file in EnumerateFiles(
            searchPath,
            searchFilter,
            searchOptions,
            accumulator
          )
        )
        {
          yield return file;
        }

        if (
          Index is 0
          && searchFilter.Length > 1
          && searchOptions is not
          {
            AttributesToSkip: System.IO.FileAttributes.System
          }
        )
        {
          searchOptions.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchPath,
              searchFilter,
              searchOptions,
              accumulator,
              !Flat
            )
          )
          {
            yield return directory;
          }

          foreach (
            var file in EnumerateFiles(
              searchPath,
              searchFilter,
              searchOptions,
              accumulator
            )
          )
          {
            yield return file;
          }

          searchOptions.AttributesToSkip = originalAttributes;
        }

        break;
    }

    if (accumulator is not "")
    {
      yield return JoinPathCompletion(
        @"\",
        accumulator
      );
    }

    if (
      accumulator is not ""
      || Index is not 0
    )
    {
      yield return JoinPathCompletion(
        @"..\",
        accumulator
      );
    }

    yield break;
  }

  private IEnumerable<string> EnumerateDirectories(
    string searchPath,
    string searchFilter,
    System.IO.EnumerationOptions searchOptions,
    string accumulatedSubpath,
    bool trailingSeparator = default
  ) => EnumerateCompletions(
     System.IO.Directory.EnumerateDirectories(
      searchPath,
      searchFilter,
      searchOptions
    ),
    accumulatedSubpath,
    trailingSeparator
  );

  private IEnumerable<string> EnumerateFiles(
    string searchPath,
    string searchFilter,
    System.IO.EnumerationOptions searchOptions,
    string accumulatedSubpath
  ) => EnumerateCompletions(
    System.IO.Directory.EnumerateFiles(
      searchPath,
      searchFilter,
      searchOptions
    ),
    accumulatedSubpath
  );

  private IEnumerable<string> EnumerateCompletions(
    IEnumerable<string> paths,
    string accumulatedSubpath,
    bool trailingSeparator = default
  )
  {
    foreach (var path in paths)
    {
      ++Index;

      yield return JoinPathCompletion(
        System.IO.Path.GetFileName(path),
        accumulatedSubpath,
        trailingSeparator
      );
    }
  }
}
