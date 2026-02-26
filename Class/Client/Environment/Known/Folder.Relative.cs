namespace Module.Client.Environment.Known;

internal static partial class Folder
{
  internal static string SystemDrive(
    string path = ""
  ) => File.FullPathLocationRelative(
    Windows(".."),
    path
  );

  internal static string Code(
    string path = ""
  ) => File.FullPathLocationRelative(
    Home("code"),
    path
  );
}
