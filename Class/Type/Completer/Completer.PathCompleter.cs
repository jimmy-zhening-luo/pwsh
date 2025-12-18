using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public enum PathProvider
    {
      Any,
      FileSystem,
      Registry,
      Environment,
      Variable,
      Alias,
      Function
    }

    public enum PathItemType
    {
      Any,
      File,
      Directory,
      RegistryKey,
      EnvironmentVariable,
      Variable,
      Alias,
      Function
    }

    public class PathCompleter : CompleterBase
    {
      public readonly string Root;
      public readonly PathItemType Type;
      public readonly bool Flat;

      public PathCompleter(
        string root,
        PathItemType? type,
        bool? flat
      ): base(CompletionCase.Preserve)
      {
        string normalizedUnescapedRoot = TypedPath.Normalize(
          Typed.Typed.Unescape(root),
          TypedPath.PathSeparator,
          true,
          true
        );

        Root = Regex.IsMatch(
            normalizedUnescapedRoot,
            TypedPath.IsPathTildeRootedPattern
          )
          ? Regex.Replace(
            normalizedUnescapedRoot,
            TypedPath.SubstituteTildeRootPattern,
            Environment.GetFolderPath(
              Environment
                .SpecialFolder
                .UserProfile
            )
          )
          : normalizedUnescapedRoot;
        Type = type ?? PathItemType.Any;
        Flat = flat ?? false;
      }

      public IEnumerable<string> FindDescendant(
        string wordToComplete
      )
      {
        string currentPathValue = TypedPath.Normalize(
          wordToComplete,
          TypedPath.PathSeparator,
          true,
          false
        );
        string currentDirectoryValue = "";
        string searchLocation = "";
        string searchFilter = "";

        if (!string.IsNullOrWhiteSpace(currentPathValue))
        {
          int lastSeparatorIndex = currentPathValue.LastIndexOf(
            TypedPath.PathSeparatorChar
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

        List<string> completions = [];

        if (Type != PathItemType.File)
        {
          string[] directories = [
            ..Directory.GetDirectories(
              searchLocation,
              searchFilter,
              attributes
            ).Select(
              directory => Path.Join(
                currentDirectoryValue,
                Path.GetFileName(directory),
                Flat
                  ? string.Empty
                  : TypedPath.PathSeparator
              )
            )
          ];

          if (directories.Length != 0)
          {
            completions.AddRange(directories);
          }
        }

        if (Type != PathItemType.Directory)
        {
          string[] files = [
            ..Directory.GetFiles(
              searchLocation,
              searchFilter,
              attributes
            ).Select(
              file => Path.Join(
                currentDirectoryValue,
                Path.GetFileName(file)
              )
            )
          ];

          if (files.Length != 0)
          {
            completions.AddRange(files);
          }
        }

        // if currentdirvalue, add dir itself
        // go-backwards ..

        return completions;
      }

      protected override IEnumerable<string> FulfillArgumentCompletion(
        string parameterName,
        string wordToComplete,
        IDictionary fakeBoundParameters
      )
      {
        foreach (
          string descendant in  FindDescendant(
            wordToComplete
          )
        )
        {
          yield return descendant.Replace(
            TypedPath.PathSeparatorChar,
            TypedPath.FriendlyPathSeparatorChar
          );
        }
      }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathLocationCompletionsAttribute(
      string Location,
      PathItemType? ItemType,
      bool? Flat
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
    {
      public IArgumentCompleter Create()
      {
        return new PathCompleter(
          Location,
          ItemType,
          Flat
        );
      }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathCompletionsAttribute(
      ScriptBlock CurrentDirectory,
      PathItemType? ItemType,
      bool? Flat
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
    {
      public IArgumentCompleter Create()
      {
        return new PathCompleter(
          CurrentDirectory
            .Invoke()[0]
            .BaseObject
            .ToString(),
          ItemType,
          Flat
        );
      }
    }
  }
}
