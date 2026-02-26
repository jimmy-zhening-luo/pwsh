namespace Module.Commands.Code.Node;

public static class NodeWorkingDirectory
{
  public static bool Test(
    string path,
    string currentLocation
  )
  {
    var workingDirectory = Client.File.PathString.Normalize(path);

    if (workingDirectory is "")
    {
      workingDirectory = currentLocation;
    }

    return System.IO.File.Exists(
      System.IO.Path.Combine(
        System.IO.Path.GetFullPath(
          workingDirectory,
          currentLocation
        ),
        "package.json"
      )
    );
  }
}
