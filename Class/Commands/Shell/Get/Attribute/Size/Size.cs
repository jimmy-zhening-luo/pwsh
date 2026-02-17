namespace Module.Commands.Shell.Get.Attribute.Size;

public sealed partial class GetSize
{
  public enum DiskSizeUnit
  {
    B,
    KB,
    MB,
    GB,
    TB,
    PB
  }

  private enum DiskSizeUnitAlias
  {
    B,
    KB,
    K = KB,
    MB,
    M = MB,
    GB,
    G = GB,
    TB,
    T = TB,
    PB,
    P = PB
  }
}
