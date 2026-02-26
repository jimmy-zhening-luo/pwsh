namespace Module.Commands.Code.Node;

public static class NodeWorkingDirectory
{
  public static bool Test(
    string currentLocation,
    string path
  ) => System.IO.File.Exists(
    System.IO.Path.Combine(
      Client.File.PathString.FullPathLocationRelative(
        currentLocation,
        path
      ),
      "package.json"
    )
  );
}
