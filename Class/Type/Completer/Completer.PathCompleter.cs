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

      protected override IEnumerable<string> FulfillArgumentCompletion(
        string parameterName,
        string wordToComplete,
        IDictionary fakeBoundParameters
      )
      {
        string currentPathValue = TypedPath.Normalize(
          wordToComplete,
          TypedPath.PathSeparator,
          true,
          false
        );

        List<string> completions = [];

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
              currentDirectoryValue[0],
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

        searchFilter = searchFilter + "*"

        List<string> matchedDirectories = [];
        List<string> matchedFiles = [];

        if (Type != PathItemType.File)
        {
          string[] directories = [
            ..Directory.GetDirectories(
              searchLocation,
              searchFilter,
              SearchOption.TopDirectoryOnly
            ).Select(
              directory => Path.Join(
                currentDirectoryValue,
                Path.GetFileName(directory)
              )
            )
          ];

          if (directories.Length != 0)
          {
            matchedDirectories.AddRange(directories);
          }
        }

        if (Type != PathItemType.Directory)
        {
          string[] files = [
            ..Directory.GetFiles(
              searchLocation,
              searchFilter,
              SearchOption.TopDirectoryOnly
            ).Select(
              file => Path.Join(
                currentDirectoryValue,
                Path.GetFileName(file)
              )
            )
          ];

          if (files.Length != 0)
          {
            matchedFiles.AddRange(files);
          }
        }

        List<string> appendedDirectories = Flat
          ? matchedDirectories
          : [
            .. matchedDirectories.Select(
              directory => directory.EndsWith(
                TypedPath.PathSeparatorChar
              )
                ? directory
                : directory + TypedPath.PathSeparator
            )
          ];

        List<string> unnormalizedCompletions = [];

        if (appendedDirectories.Count != 0)
        {
          unnormalizedCompletions.AddRange(appendedDirectories);
        }
        if (matchedFiles.Count != 0)
        {
          unnormalizedCompletions.AddRange(matchedFiles);
        }

        if (unnormalizedCompletions.Count != 0)
        {
          completions.AddRange(
            unnormalizedCompletions.Select(
              item => item.Replace(
                TypedPath.PathSeparatorChar,
                TypedPath.FriendlyPathSeparatorChar
              )
            )
          );
        }

        return completions;
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
