namespace Module.Command.Shell.Get.Attribute;

public enum DiskSizeUnit
{
  B,
  KB,
  MB,
  GB,
  TB,
  PB
}

public enum DiskSizeAlias
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

public class DiskSize
{
  public static readonly Dictionary<DiskSizeUnit, long> Factor = new() {
      { DiskSizeUnit.B, 1L },
      { DiskSizeUnit.KB, 1L << 10 },
      { DiskSizeUnit.MB, 1L << 20 },
      { DiskSizeUnit.GB, 1L << 30 },
      { DiskSizeUnit.TB, 1L << 40 },
      { DiskSizeUnit.PB, 1L << 50 }
    };
}
