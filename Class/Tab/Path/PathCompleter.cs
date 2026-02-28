namespace Module.Tab.Path;

internal sealed class PathCompleter : TabCompleter
{
  private record SearchContext(
    string Path,
    string Filter,
    System.IO.EnumerationOptions Options
  );

  private readonly string Location;

  private readonly PathItemType ItemType;

  private readonly bool Flat;

  private readonly bool IncludeHidden;

  private readonly bool AllowReanchor;

  private uint Index;

  internal PathCompleter(
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

  private static (SearchContext, string) ParsePathToComplete(
    string wordToComplete,
    string location,
    bool allowReanchor,
    bool includeHidden
  )
  {
    var normalWordToComplete = Client.File.PathString.Normalize(
      wordToComplete,
      true
    );
    var resolvedSearchPath = string.Empty;
    var normalWordToCompleteSubpathPart = string.Empty;
    var normalWordToCompleteLeafPart = string.Empty;

    while (normalWordToComplete is not "")
    {
      var indexLastDirectorySeparator = normalWordToComplete.LastIndexOf('\\');

      if (indexLastDirectorySeparator < 0)
      {
        normalWordToCompleteLeafPart = normalWordToComplete;
        normalWordToComplete = string.Empty;
      }
      else
      {
        var candidateSubpathPart = normalWordToComplete[..indexLastDirectorySeparator].Trim();
        var indexNormalPathToCompleteLeafPart = indexLastDirectorySeparator + 1;

        if (indexNormalPathToCompleteLeafPart < normalWordToComplete.Length)
        {
          normalWordToCompleteLeafPart = normalWordToComplete[indexNormalPathToCompleteLeafPart..].Trim();
        }

        if (candidateSubpathPart is "")
        {
          normalWordToComplete = string.Empty;
        }
        else
        {
          var candidateResolvedSearchPath = System.IO.Path.GetFullPath(
            candidateSubpathPart,
            location
          );

          if (
            System.IO.Directory.Exists(
              candidateResolvedSearchPath
            )
          )
          {
            normalWordToCompleteSubpathPart = System.IO.Path.GetRelativePath(
              location,
              candidateResolvedSearchPath
            );
            resolvedSearchPath = candidateResolvedSearchPath;
            normalWordToComplete = string.Empty;
          }
          else if (
            allowReanchor
            && System.IO.Directory.Exists(candidateSubpathPart)
          )
          {
            normalWordToCompleteSubpathPart = System.IO.Path.GetFullPath(candidateSubpathPart);
            resolvedSearchPath = normalWordToCompleteSubpathPart;
            normalWordToComplete = string.Empty;
          }
          else
          {
            normalWordToCompleteLeafPart = string.Empty;
            normalWordToComplete = candidateSubpathPart;
          }
        }
      }
    }

    if (resolvedSearchPath is "")
    {
      resolvedSearchPath = location;
    }

    System.IO.EnumerationOptions searchOptions = new()
    {
      IgnoreInaccessible = default
    };
    if (includeHidden)
    {
      searchOptions.AttributesToSkip = System.IO.FileAttributes.System;
    }

    return (
      new(
        resolvedSearchPath,
        normalWordToCompleteLeafPart + "*",
        searchOptions
      ),
      normalWordToCompleteSubpathPart
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
      searchContext,
      accumulator
    ) = ParsePathToComplete(
      wordToComplete,
      Location,
      AllowReanchor,
      IncludeHidden
    );

    var originalAttributes = searchContext.Options.AttributesToSkip;

    switch (ItemType)
    {
      case PathItemType.Directory:
        foreach (
          var directory in EnumerateDirectories(
            searchContext,
            accumulator,
            !Flat
          )
        )
        {
          yield return directory;
        }

        if (
          Index is 0
          && searchContext is
          {
            Filter.Length: > 1,
            Options.AttributesToSkip: not System.IO.FileAttributes.System,
          }
        )
        {
          searchContext.Options.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchContext,
              accumulator,
              !Flat
            )
          )
          {
            yield return directory;
          }

          searchContext.Options.AttributesToSkip = originalAttributes;
        }

        break;

      case PathItemType.File:
        foreach (
          var file in EnumerateFiles(
            searchContext,
            accumulator
          )
        )
        {
          yield return file;
        }

        if (
          Index is 0
          && searchContext is
          {
            Filter.Length: > 1,
            Options.AttributesToSkip: not System.IO.FileAttributes.System,
          }
        )
        {
          searchContext.Options.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var file in EnumerateFiles(
              searchContext,
              accumulator
            )
          )
          {
            yield return file;
          }

          searchContext.Options.AttributesToSkip = originalAttributes;
        }

        var checkpoint = Index;

        foreach (
          var directory in EnumerateDirectories(
            searchContext,
            accumulator,
            true
          )
        )
        {
          yield return directory;
        }

        if (
          Index == checkpoint
          && searchContext is
          {
            Filter.Length: > 1,
            Options.AttributesToSkip: not System.IO.FileAttributes.System,
          }
        )
        {
          searchContext.Options.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchContext,
              accumulator,
              true
            )
          )
          {
            yield return directory;
          }

          searchContext.Options.AttributesToSkip = originalAttributes;
        }

        break;

      default:
        foreach (
          var directory in EnumerateDirectories(
            searchContext,
            accumulator,
            !Flat
          )
        )
        {
          yield return directory;
        }

        foreach (
          var file in EnumerateFiles(
            searchContext,
            accumulator
          )
        )
        {
          yield return file;
        }

        if (
          Index is 0
          && searchContext is
          {
            Filter.Length: > 1,
            Options.AttributesToSkip: not System.IO.FileAttributes.System,
          }
        )
        {
          searchContext.Options.AttributesToSkip = System.IO.FileAttributes.System;

          foreach (
            var directory in EnumerateDirectories(
              searchContext,
              accumulator,
              !Flat
            )
          )
          {
            yield return directory;
          }

          foreach (
            var file in EnumerateFiles(
              searchContext,
              accumulator
            )
          )
          {
            yield return file;
          }

          searchContext.Options.AttributesToSkip = originalAttributes;
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
    SearchContext searchContext,
    string accumulator,
    bool trailingSeparator = default
  ) => EnumerateCompletions(
     System.IO.Directory.EnumerateDirectories(
      searchContext.Path,
      searchContext.Filter,
      searchContext.Options
    ),
    accumulator,
    trailingSeparator
  );

  private IEnumerable<string> EnumerateFiles(
    SearchContext searchContext,
    string accumulator
  ) => EnumerateCompletions(
    System.IO.Directory.EnumerateFiles(
      searchContext.Path,
      searchContext.Filter,
      searchContext.Options
    ),
    accumulator
  );

  private IEnumerable<string> EnumerateCompletions(
    IEnumerable<string> paths,
    string accumulator,
    bool trailingSeparator = default
  )
  {
    foreach (var path in paths)
    {
      ++Index;

      yield return JoinPathCompletion(
        System.IO.Path.GetFileName(path),
        accumulator,
        trailingSeparator
      );
    }
  }
}
