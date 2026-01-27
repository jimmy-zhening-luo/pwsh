global using static System.IO.Path;
global using static Module.Context;
global using static Module.Input.Escaper;
global using static Module.Input.PathResolution.Canonicalizer;

global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Management.Automation;
global using Module.Input.PathResolution;
global using Module.Completer;
global using Module.Completer.PathCompleter;

global using IO = System.IO;
global using CodeAnalysis = System.Diagnostics.CodeAnalysis;
global using Language = System.Management.Automation.Language;

global using Environment = System.Environment;
global using Enum = System.Enum;
global using Uri = System.Uri;
global using StringComparer = System.StringComparer;
global using StringComparison = System.StringComparison;
global using StringSplitOptions = System.StringSplitOptions;
global using AttributeUsage = System.AttributeUsageAttribute;
global using AttributeTargets = System.AttributeTargets;
global using IDictionary = System.Collections.IDictionary;
global using Directory = System.IO.Directory;
global using File = System.IO.File;
global using DirectoryInfo = System.IO.DirectoryInfo;
global using FileInfo = System.IO.FileInfo;
global using FileAttributes = System.IO.FileAttributes;
global using EnumerationOptions = System.IO.EnumerationOptions;
global using SearchOption = System.IO.SearchOption;
global using Process = System.Diagnostics.Process;
global using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;
