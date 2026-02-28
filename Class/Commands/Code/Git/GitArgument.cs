namespace Module.Commands.Code.Git;

[System.Obsolete("Removing")]
internal static partial class GitArgument
{
  [System.Obsolete("Removing")]
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$"
  )]
  internal static partial System.Text.RegularExpressions.Regex Regex();

  [System.Obsolete("Removing")]
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?=.)(?>HEAD)?(?<branching>(?>~|\^)?)(?<step>(?>\d{0,10}))$"
  )]
  internal static partial System.Text.RegularExpressions.Regex TreeRegex();
}
