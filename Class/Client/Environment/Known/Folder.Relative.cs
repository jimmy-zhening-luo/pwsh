namespace PowerModule.Client.Environment.Known;

static internal partial class Folder
{
  static internal string SystemDrive(string path) => File.PathString.FullPathLocationRelative(
    SystemDrive(),
    path
  );
  static internal string SystemDrive() => system ??= Windows("..");
  static private string? system;

  static internal string Code(string path) => File.PathString.FullPathLocationRelative(
    Code(),
    path
  );
  static internal string Code() => code ??= Home("code");
  static private string? code;
}
