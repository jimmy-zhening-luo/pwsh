namespace Module.Commands.Code.Node;

public static class NodeWorkingDirectory
{
  public static bool Test(
    string path,
    string currentDirectory
  )
  {
    var workingDirectory = Client.File.PathString.Normalize(path);

    if (workingDirectory is "")
    {
      workingDirectory = currentDirectory;
    }

    return System.IO.File.Exists(
      System.IO.Path.Join(
        workingDirectory,
        "package.json"
      )
    );
  }
}
