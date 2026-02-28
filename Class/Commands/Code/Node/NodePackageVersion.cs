namespace Module.Commands.Code.Node;

public enum NodePackageVersion
{
  patch,
  minor,
  major,
  prerelease,
  prepatch = prerelease,
  preminor,
  premajor,
}
