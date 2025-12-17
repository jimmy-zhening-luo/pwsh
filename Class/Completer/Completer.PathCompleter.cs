using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;
using Typed;

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

    public class TestPathCompleter : CompleterBase
    {
      private readonly string Root;
      private readonly PathItemType Type;
      private readonly bool Flat;
      private readonly bool UseNativePathSeparator;

      public TestPathCompleter(
        string root,
        PathItemType type,
        bool flat,
        bool useNativePathSeparator
      )
      {
        Root = root;
        Type = type;
        Flat = flat;
        UseNativePathSeparator = useNativePathSeparator;
      }

      public override List<string> FulfillCompletion(
        string parameterName,
        string wordToComplete,
        IDictionary fakeBoundParameters
      )
      {
        List<string> completions = [];

        string fullRoot = Path.GetFullPath(Root);

        bool constrainToDirectories = (
          Type == PathItemType.Directory
        );
        bool constrainToFiles = (
          Type == PathItemType.File
        );

        string currentPathValue = Regex.Replace(
          Typed.Typed
            .Unescape(
              wordToComplete
            )
            .Replace(
              TypedPath.FriendlyPathSeparatorChar,
              TypedPath.PathSeparatorChar
            ),
          TypedPath.DuplicatePathSeparatorPattern,
          TypedPath.PathSeparator
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
            string testPath = Path.GetFullPath(currentDirectoryValue[0], fullRoot);

            if (Directory.Exists(testPath))
            {
              searchLocation.Add(testPath);

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
          searchLocation.Add(fullRoot);
        }

        if (searchFilter.Count == 0)
        {
          searchFilter.Add("*");
        }

        List<string> matchedDirectoryNames = [];
        List<string> matchedDirectoryFiles = [];

        if (constrainToDirectories || !constrainToFiles)
        {
          string[] directories = Directory.GetDirectories(
            searchLocation[0],
            searchFilter[0],
            SearchOption.TopDirectoryOnly
          ).Select(
            d => Path.GetFileName(d)
          ).ToArray();

          if (directories.Length != 0)
          {
            matchedDirectoryNames.AddRange(directories);
          }
        }

        if (constrainToFiles || !constrainToDirectories)
        {
          string[] files = Directory.GetFiles(
            searchLocation[0],
            searchFilter[0],
            SearchOption.TopDirectoryOnly
          ).Select(
            f => Path.GetFileName(f)
          ).ToArray();

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
  }
}
