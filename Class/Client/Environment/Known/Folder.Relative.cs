namespace Module.Client.Environment.Known;

internal static partial class Folder
{
  internal static string SystemDrive(string path) => File.PathString.FullPathLocationRelative(
    SystemDrive(),
    path
  );
  internal static string SystemDrive() => system ??= Windows("..");
  private static string? system;

  internal static string Code(string path) => File.PathString.FullPathLocationRelative(
    Code(),
    path
  );
  internal static string Code() => code ??= Home("code");
  private static string? code;
}
