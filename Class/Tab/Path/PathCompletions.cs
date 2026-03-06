namespace Module.Tab.Path;

internal class PathCompletionsAttribute(
  string Location = "",
  PathItemType ItemType = default,
  bool Flat = default
) : TabCompletionsAttribute
{
  public sealed override PathCompleter Create() => new(
    Location,
    ItemType,
    Flat
  );

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
    private readonly bool AllowReanchor;

    private uint Index;

    internal PathCompleter(
      string location,
      PathItemType itemType,
      bool flat
    ) : base(
      default,
      CompletionResultType.ProviderItem
    ) => (
      Location,
      ItemType,
      Flat,
      AllowReanchor
    ) = (
      Client.File.PathString.Normalize(location) is var normalPath
      && System.IO.Path.IsPathFullyQualified(normalPath)
        ? normalPath
        : Module.FullPathCurrentLocationRelative(normalPath),
      itemType,
      flat,
      location is ""
    );

    private static (string, SearchContext) ParseLine(
      string wordToComplete,
      string location,
      bool allowReanchor
    )
    {
      var searchPath = location;
      var line = Client.File.PathString.Normalize(
        wordToComplete,
        true
      );
      var lineCaptured = string.Empty;
      var lineRemaining = string.Empty;

      while (line is not "")
      {
        var marker = line.LastIndexOf(
          Client.File.PathString.Separator
        );

        if (marker < 0)
        {
          (
            lineRemaining,
            line
          ) = (
            line,
            string.Empty
          );
        }
        else
        {
          var buffer = line[..marker];
          var next = marker + 1;

          if (next < line.Length)
          {
            lineRemaining = line[next..];
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

            if (
              System.IO.Directory.Exists(
                fullPathCaptured
              )
            )
            {
              (
                searchPath,
                lineCaptured,
                line
              ) = (
                fullPathCaptured,
                System.IO.Path.GetRelativePath(
                  location,
                  fullPathCaptured
                ),
                string.Empty
              );
            }
            else if (
              allowReanchor
              && System.IO.Directory.Exists(
                buffer
              )
            )
            {
              (
                searchPath,
                line
              ) = (
                lineCaptured = System.IO.Path.GetFullPath(
                  buffer
                ),
                string.Empty
              );
            }
            else
            {
              (
                lineRemaining,
                line
              ) = (
                string.Empty,
                buffer
              );
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
            AttributesToSkip = System.IO.FileAttributes.System,
          }
        )
      );
    }

    private static CompletionResultRecord CreateCompletionRecord(
      string description,
      string accumulator,
      string filename,
      bool trailingSeparator = default
    ) => new(
      System.IO.Path.Join(
        accumulator,
        filename,
        trailingSeparator
          ? Client.File.PathString.SeparatorString
          : string.Empty
      )
        .Replace(
          Client.File.PathString.Separator,
          Client.File.PathString.AltSeparator
        ),
      Description: description
    );

    private protected sealed override IEnumerable<CompletionResultRecord> GenerateCompletion(string wordToComplete)
    {
      Index = default;

      var (accumulator, searchContext) = ParseLine(
        wordToComplete,
        Location,
        AllowReanchor
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

          break;
      }

      if (accumulator is not "")
      {
        yield return CreateCompletionRecord(
          System.IO.Path.Join(
            searchContext.Path,
            Client.File.PathString.SeparatorString
          ),
          accumulator,
          Client.File.PathString.SeparatorString
        );
      }

      if (accumulator is not "" || Index is not 0)
      {
        yield return CreateCompletionRecord(
          Client.File.PathString.FullPathLocationRelative(
            searchContext.Path,
            ".."
          ),
          accumulator,
          @"..\"
        );
      }

      yield break;
    }

    private IEnumerable<CompletionResultRecord> Directories(
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

    private IEnumerable<CompletionResultRecord> Files(
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

    private IEnumerable<CompletionResultRecord> EnumerateResults(
      string accumulator,
      IEnumerable<string> paths,
      bool trailingSeparator = default
    )
    {
      foreach (var path in paths)
      {
        ++Index;

        yield return CreateCompletionRecord(
          path,
          accumulator,
          System.IO.Path.GetFileName(path),
          trailingSeparator
        );
      }
      yield break;
    }
  }
}
