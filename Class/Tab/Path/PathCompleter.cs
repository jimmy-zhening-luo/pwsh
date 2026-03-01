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

  private static (string, SearchContext) ParseLine(
    string wordToComplete,
    string location,
    bool allowReanchor,
    bool includeHidden
  )
  {
    var line = Client.File.PathString.Normalize(
      wordToComplete,
      true
    );
    var lineRemaining = string.Empty;
    var lineCaptured = string.Empty;
    var searchPath = location;
    
    while (line is not "")
    {
      var marker = line.LastIndexOf('\\');

      if (marker < 0)
      {
        (line, lineRemaining) = ("", line);
      }
      else
      {
        var buffer = line[..marker].Trim();
        var next = marker + 1;

        if (next < line.Length)
        {
          lineRemaining = line[next..].Trim();
        }

        if (buffer is "")
        {
          line = string.Empty;
        }
        else
        {
          var fullPathCaptured = Client.File.PathString.FullPathLocationRelative(
            location,
            buffer
          );

          if (System.IO.Directory.Exists(fullPathCaptured))
          {
            line = string.Empty;
            lineCaptured = System.IO.Path.GetRelativePath(
              location,
              fullPathCaptured
            );
            searchPath = fullPathCaptured;
          }
          else if (
            allowReanchor
            && System.IO.Directory.Exists(buffer)
          )
          {
            line = string.Empty;
            lineCaptured = System.IO.Path.GetFullPath(buffer);
            searchPath = lineCaptured;
          }
          else
          {
            line = buffer;
            lineRemaining = string.Empty;
          }
        }
      }
    }

    return (
      lineCaptured,
      new(
        searchPath,
        lineRemaining + "*",
        new()
        {
          IgnoreInaccessible = default,
          AttributesToSkip = System.IO.FileAttributes.System
            | (
              includeHidden
                ? 0
                : System.IO.FileAttributes.Hidden
            ),
        }
      )
    );
  }

  private static string Join(
    string accumulator,
    string filename,
    bool trailingSeparator = default
  ) => System.IO.Path.Join(
    accumulator,
    filename,
    trailingSeparator
      ? @"\"
      : string.Empty
  )
    .Replace('\\', '/');

  private protected sealed override IEnumerable<string> GenerateCompletion(string wordToComplete)
  {
    Index = default;

    var (accumulator, searchContext) = ParseLine(
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
          var directory in Directories(
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
            var directory in Directories(
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
          var file in Files(
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
            var file in Files(
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
          var directory in Directories(
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
            var directory in Directories(
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
          var directory in Directories(
            searchContext,
            accumulator,
            !Flat
          )
        )
        {
          yield return directory;
        }

        foreach (
          var file in Files(
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
            var directory in Directories(
              searchContext,
              accumulator,
              !Flat
            )
          )
          {
            yield return directory;
          }

          foreach (
            var file in Files(
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
      yield return Join(accumulator, @"\");
    }

    if (
      accumulator is not ""
      || Index is not 0
    )
    {
      yield return Join(accumulator, @"..\");
    }

    yield break;
  }

  private IEnumerable<string> Directories(
    SearchContext searchContext,
    string accumulator,
    bool trailingSeparator = default
  ) => EnumerateResults(
    accumulator,
    System.IO.Directory.EnumerateDirectories(
      searchContext.Path,
      searchContext.Filter,
      searchContext.Options
    ),
    trailingSeparator
  );

  private IEnumerable<string> Files(
    SearchContext searchContext,
    string accumulator
  ) => EnumerateResults(
    accumulator,
    System.IO.Directory.EnumerateFiles(
      searchContext.Path,
      searchContext.Filter,
      searchContext.Options
    )
  );

  private IEnumerable<string> EnumerateResults(
    string accumulator,
    IEnumerable<string> paths,
    bool trailingSeparator = default
  )
  {
    foreach (var path in paths)
    {
      ++Index;
      yield return Join(
        accumulator,
        System.IO.Path.GetFileName(path),
        trailingSeparator
      );
    }
  }
}
