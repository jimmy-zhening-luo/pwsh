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
      public readonly string CurrentDirectory;
      public readonly string Root;
      public readonly PathItemType Type;
      public readonly bool Flat;
      public readonly bool UseNativePathSeparator;

      public PathCompleter(
        string currentDirectory,
        string root,
        PathItemType type,
        bool flat,
        bool useNativePathSeparator
      ): base(CompletionCase.Preserve)
      {
        string normalizedUnescapedRoot = TypedPath.Normalize(
          Typed.Typed.Unescape(root),
          TypedPath.PathSeparator,
          true,
          true
        );

        string untildedNormalizedEscapedRoot = Regex.IsMatch(
            normalizedUnescapedRoot,
            TypedPath.IsPathTildeRootedPattern
          )
          ? Regex.Replace(
            normalizedUnescapedRoot,
            TypedPath.SubstituteTildeRootPattern,
            Environment.GetFolderPath(
              Environment.SpecialFolder.UserProfile
            )
          )
          : normalizedUnescapedRoot;

        string undottedUntildedNormalizedEscapedRoot = untildedNormalizedEscapedRoot.Contains(value: ':')
        || untildedNormalizedEscapedRoot.StartsWith(
          TypedPath.PathSeparator
        )
          ? untildedNormalizedEscapedRoot
          : untildedNormalizedEscapedRoot == string.Empty
            ? currentDirectory
            : currentDirectory
              + TypedPath.PathSeparator
              + untildedNormalizedEscapedRoot;

        CurrentDirectory = currentDirectory;
        Root = undottedUntildedNormalizedEscapedRoot;
        Type = type;
        Flat = flat;
        UseNativePathSeparator = useNativePathSeparator;
      }

      public override IEnumerable<string> FulfillCompletion(
        string parameterName,
        string wordToComplete,
        IDictionary fakeBoundParameters
      )
      {
        List<string> completions = [];

        bool constrainToDirectories = (
          Type == PathItemType.Directory
        );
        bool constrainToFiles = (
          Type == PathItemType.File
        );

        string currentPathValue = TypedPath.Normalize(
          wordToComplete,
          TypedPath.PathSeparator,
          true,
          false
        );

        List<string> currentDirectoryValue = [];
        List<string> fragment = [];
        List<string> searchLocation = [];
        List<string> searchFilter = [];

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
              currentDirectoryValue.Add(beforeSeparator);
            }

            if (!string.IsNullOrWhiteSpace(afterSeparator))
            {
              fragment.Add(afterSeparator);
            }
          }
          else
          {
            fragment.Add(currentPathValue);
          }

          if (currentDirectoryValue.Count != 0)
          {
            string fullPathAndCurrentDirectory = Path.GetFullPath(currentDirectoryValue[0], Root);

            if (Directory.Exists(fullPathAndCurrentDirectory))
            {
              searchLocation.Add(fullPathAndCurrentDirectory);

            }
          }

          if (fragment.Count != 0)
          {
            searchFilter.Add(
              fragment[0] + "*"
            );
          }
        }

        if (searchLocation.Count == 0)
        {
          searchLocation.Add(Root);
        }

        if (searchFilter.Count == 0)
        {
          searchFilter.Add("*");
        }

        List<string> matchedDirectoryNames = [];
        List<string> matchedDirectoryFiles = [];

        if (constrainToDirectories || !constrainToFiles)
        {
          string[] directories = [
            ..Directory.GetDirectories(
              searchLocation[0],
              searchFilter[0],
              SearchOption.TopDirectoryOnly
            ).Select(
              d => Path.GetFileName(d)
            )
          ];

          if (directories.Length != 0)
          {
            matchedDirectoryNames.AddRange(directories);
          }
        }

        if (constrainToFiles || !constrainToDirectories)
        {
          string[] files = [
            ..Directory.GetFiles(
              searchLocation[0],
              searchFilter[0],
              SearchOption.TopDirectoryOnly
            ).Select(
              f => Path.GetFileName(f)
            )
          ];

          if (files.Length != 0)
          {
            matchedDirectoryFiles.AddRange(files);
          }
        }

        string prepender = currentDirectoryValue.Count == 0
          ? string.Empty
          : currentDirectoryValue[0] + TypedPath.PathSeparator;

        List<string> prependedDirectories = prepender == string.Empty
          ? matchedDirectoryNames
          : [
            .. matchedDirectoryNames.Select(
              directory => prepender + directory
            )
          ];
        List<string> prependedFiles = prepender == string.Empty
          ? matchedDirectoryFiles
          : [
            .. matchedDirectoryFiles.Select(
              file => prepender + file
            )
          ];

        List<string> appendedDirectories = Flat
          ? prependedDirectories
          : [
            .. prependedDirectories.Select(
              directory => directory.EndsWith(
                TypedPath.PathSeparator
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
        if (prependedFiles.Count != 0)
        {
          unnormalizedCompletions.AddRange(prependedFiles);
        }

        if (unnormalizedCompletions.Count != 0)
        {
          completions.AddRange(
            UseNativePathSeparator
              ? unnormalizedCompletions
              : unnormalizedCompletions.Select(
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
      string Root,
      PathItemType? ItemType,
      bool? Flat,
      bool? UseNativePathSeparator
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
    {
      public IArgumentCompleter Create()
      {
        return new PathCompleter(
          "",
          Root,
          ItemType ?? PathItemType.Any,
          Flat ?? false,
          UseNativePathSeparator ?? false
        );
      }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathCompletionsAttribute(
      ScriptBlock CurrentDirectory,
      PathItemType? ItemType,
      bool? Flat,
      bool? UseNativePathSeparator
    ) : ArgumentCompleterAttribute, IArgumentCompleterFactory
    {
      public IArgumentCompleter Create()
      {
        var resolved = CurrentDirectory.Invoke();

        return new PathCompleter(
          resolved[0].BaseObject.ToString(),
          ".",
          ItemType ?? PathItemType.Any,
          Flat ?? false,
          UseNativePathSeparator ?? false
        );
      }
    }
  }
}
