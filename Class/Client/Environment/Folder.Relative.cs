namespace PowerModule.Client.Environment;

partial class Folder
{
  static internal string SystemDrive(string path) => File.PathString.GetFullPathLocal(
    SystemDrive(),
    path
  );
  static internal string SystemDrive() => system ??= Windows(File.PathString.Parent);
  static string? system;

  static internal string Code(string path) => File.PathString.GetFullPathLocal(
    Code(),
    path
  );
  static internal string Code() => code ??= Home("code");
  static string? code;
}
