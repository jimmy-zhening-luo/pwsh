namespace Module.Tab.Path;

using Attributes = System.IO.FileAttributes;

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
      System.IO.DirectoryInfo Container,
      string Filter,
      System.IO.EnumerationOptions Options,
      string Accumulator
    );

    private readonly string Location;
    private readonly PathItemType ItemType;
    private readonly bool Flat;
    private readonly bool AllowReanchor;

    private bool matched;

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

    private static SearchContext ParseLine(
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
        new(
          new(searchPath),
          lineRemaining + "*",
          new()
          {
            IgnoreInaccessible = default,
            AttributesToSkip = Attributes.NotContentIndexed,
          },
          lineCaptured
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
      matched = default;

      var searchContext = ParseLine(
        wordToComplete,
        Location,
        AllowReanchor
      );

      switch (ItemType)
      {
        case PathItemType.Directory:
          foreach (
            var directory in Directories(
              searchContext,
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
              searchContext
            )
          )
          {
            yield return file;
          }

          foreach (
            var directory in Directories(
              searchContext,
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
              !Flat
            )
          )
          {
            yield return directory;
          }

          foreach (
            var file in Files(
              searchContext
            )
          )
          {
            yield return file;
          }

          break;
      }

      if (searchContext.Accumulator is not "")
      {
        yield return CreateCompletionRecord(
          System.IO.Path.Join(
            searchContext.Container.FullName,
            Client.File.PathString.SeparatorString
          ),
          searchContext.Accumulator,
          Client.File.PathString.SeparatorString
        );
      }

      if (matched || searchContext.Accumulator is not "")
      {
        yield return CreateCompletionRecord(
          Client.File.PathString.FullPathLocationRelative(
            searchContext.Container.FullName,
            ".."
          ),
          searchContext.Accumulator,
          @"..\"
        );
      }

      yield break;
    }

    private IEnumerable<CompletionResultRecord> Directories(
      SearchContext searchContext,
      bool trailingSeparator = default
    ) => EnumerateResults(
      searchContext.Container.EnumerateDirectories(
        searchContext.Filter,
        searchContext.Options
      ),
      searchContext.Accumulator,
      trailingSeparator
    );

    private IEnumerable<CompletionResultRecord> Files(
      SearchContext searchContext
    ) => EnumerateResults(
      searchContext.Container.EnumerateFiles(
        searchContext.Filter,
        searchContext.Options
      ),
      searchContext.Accumulator
    );

    private IEnumerable<CompletionResultRecord> EnumerateResults(
      IEnumerable<System.IO.FileSystemInfo> items,
      string accumulator,
      bool trailingSeparator = default
    )
    {
      List<System.IO.FileSystemInfo> deferredItems = [];

      foreach (var item in items)
      {
        switch (
          (
            item.Attributes & Attributes.Hidden,
            item.Attributes & Attributes.System
          )
        )
        {
          case (not 0, not 0):
            break;

          case (not 0, _):
            deferredItems.Add(item);
            break;

          default:
            matched = true;

            yield return CreateCompletionRecord(
              item.FullName,
              accumulator,
              item.Name,
              trailingSeparator
            );
            break;
        }
      }

      foreach (var item in deferredItems)
      {
        matched = true;

        yield return CreateCompletionRecord(
          item.FullName,
          accumulator,
          item.Name,
          trailingSeparator
        );
      }

      yield break;
    }
  }
}
