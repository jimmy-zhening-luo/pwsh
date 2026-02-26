namespace Module.Client.Environment.Known;

internal static partial class Folder
{
  internal static string SystemDrive(
    string path = ""
  ) => File.PathString.FullPathLocationRelative(
    Windows(".."),
    path
  );

  internal static string Code(
    string path = ""
  ) => File.PathString.FullPathLocationRelative(
    Home("code"),
    path
  );
}
